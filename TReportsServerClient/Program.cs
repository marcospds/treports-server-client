using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Totvs.Licence.Client;
using TOTVS.Licence.LSCloud;

namespace TReportsServerClient
{
  public class Program
  {
    private static string RacUrl = "http://localhost/totvs.rac";
    private static string ClientId = "treports_ro";
    private static string ClientSecret = "totvs@123";

    private static string UserName = "teste";
    private static string Password = "teste@123";


    private static string TReportsUrl = "http://localhost/treports";

    private static string UidReport = "0de072a9-4e6f-4ee8-b5bc-f562b538dada";
    private static string UserNameExecuter = "usuarioLogado";

    private static string FileDirectory = @"C:\Temp\TReports.pdf";

    public static async Task Main(string[] args)
    {
      // Get Access Token (Resource Owner)
      var token = await GetResourceOwnerToken(RacUrl, ClientId, ClientSecret, UserName, Password);

      // Get Uid Execution Request 
      var UidRequest = await GetUidRequest(token, UidReport, UserNameExecuter);

      // Wait Report Execution and Get Id to Download the file
      var Id = await GetIdRequest(token, UidRequest);

      // Download the File and Save
      await GetFile(token, Id, FileDirectory);

      Console.WriteLine($"File Save: {FileDirectory}");

      Console.Read();
    }

    private static async Task<string> GetResourceOwnerToken(string racUrl, string clientId, string clientSecret, string userName, string password)
    {
      var client = new HttpClient();
      var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
      {
        Address = $"{racUrl}/connect/token",

        ClientId = clientId,
        ClientSecret = clientSecret,
        Scope = "authorization_api",

        UserName = userName,
        Password = password
      });

      if (response.IsError) throw new Exception(response.Error);

      Console.WriteLine("Get Access Token successeul!");

      return response.AccessToken;
    }

    private static async Task<string> GetUidRequest(string token, string uIdReport, string userNameExecuter)
    {
      // Config generation parameters
      GenerateReportDto generateReport = new GenerateReportDto
      {
        GenerateParams = new GenerateParams
        {
          IsView = false,
          StopExecutionOnError = true,
          Parameters = new List<ParameterParam>()
        },
        ScheduleParams = new ScheduleParams
        {
          Type = TypeEnum.tNow
        },
        // Pass UserName for Provider Security
        User = userNameExecuter
      };

      using (HttpClient httpClient = new HttpClient())
      {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await httpClient.PostAsync(
          $"{TReportsUrl}/api/trep/v1/reports/{uIdReport}/execute",
          new StringContent(
            JsonConvert.SerializeObject(generateReport),
            Encoding.UTF8,
            "application/json"
            ));

        var resultContent = result.Content.ReadAsStringAsync().Result;
        var reportReturnDto = JsonConvert.DeserializeObject<GenerateReportReturnDto>(resultContent);

        Console.WriteLine($"UidRequest: {reportReturnDto.UIdRequest}");

        return reportReturnDto.UIdRequest;
      }
    }

    private static async Task<int> GetIdRequest(string token, string uIdRequest)
    {
      using (HttpClient httpClient = new HttpClient())
      {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Repeats until execution or report failure
        while (true)
        {
          var result = await httpClient.GetAsync($"{TReportsUrl}/api/trep/v1/generateds/{uIdRequest}/info");

          // Se não retornou sucesso levanta a exceção
          if (!result.IsSuccessStatusCode)
          {
            throw new Exception("Problema ao recuperar informações do relatório gerado!");
          }
          var resultContent = result.Content.ReadAsStringAsync().Result;

          if (string.IsNullOrEmpty(resultContent))
          {
            // Wait 2 seconds
            System.Threading.Thread.Sleep(2000);
            continue;
          }
          var executionSchedule = JsonConvert.DeserializeObject<ExecutionScheduleDto>(resultContent);

          Console.WriteLine($"Status: {executionSchedule.Status}");

          //if (executionSchedule.Status == ExecutionScheduleStatusValues.Pending ||
          //    executionSchedule.Status == ExecutionScheduleStatusValues.Running)
          if (executionSchedule.Status != "Finalizado")
          {
            // Wait 5 seconds
            System.Threading.Thread.Sleep(5000);
            continue;
          }

          Console.WriteLine($"Id: {executionSchedule.Id}. Status {executionSchedule.Status}");
          return executionSchedule.Id;
        }
      }
    }

    private static async Task GetFile(string token, int id, string fileDirecotry)
    {
      using (WebClient webClient = new WebClient())
      {
        webClient.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {token}");

        await webClient.DownloadFileTaskAsync(new Uri($"{TReportsUrl}/api/trep/v1/generateds/{id}/file?exportFileType={ExportFileTypeEnum.PDF.GetHashCode()}"), fileDirecotry);
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TReportsServerClient
{
  public class Report
  {
    public int id { get; set; }
    public string uId { get; set; }
  }

  /// <summary>
  /// Dados de request para solicitação de geração do relatórios.
  /// </summary>
  public class GenerateReportDto
  {
    /// <summary>
    /// Identificador da solicitação
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Usuáiro de solicitação.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// Parametros de geração do relatório.
    /// </summary>
    public GenerateParams GenerateParams { get; set; }

    /// <summary>
    /// Dados de schedule de execução do relatório.
    /// </summary>
    public ScheduleParams ScheduleParams { get; set; }
  }

  public class ScheduleParams
  {
    /// <summary>
    ///0- Agora - A execução ocorrerá imediatamente. (valor default).
    ///1- Data específica- deverá ser informado uma data na propriedade "Date".
    /// </summary>
    public TypeEnum Type
    {
      get { return _Type; }
      set { _Type = value; }
    }
    private TypeEnum _Type = TypeEnum.tNow;

    public string GetTypeValue()
    {
      return ((int)Type).ToString();
    }

    /// <summary>
    /// Deve ser informado se for informado o TypeEnum.tDate
    /// </summary>
    public DateTimeOffset? Date { get; set; }

    /// <summary>
    /// Informa se a execução vai repetir.
    /// O default é "false". a execução não se repetirá.
    /// </summary>
    public bool Repeat
    {
      get { return _Repeat; }
      set { _Repeat = value; }
    }
    private bool _Repeat = false;

    /// <summary>
    /// Se a execução for repetir, informa como ela se repetirá 
    /// 0- Diariamente - a cada intervalo em horas. A hora será recuperada da propriedade "RepeatTime"
    /// 1- Semanalmente - Pode escolher os dias da semana. 
    /// 2- Mensalmente - Será executada mensalmente no dia informado na propriedade "RepeatMonth.Day"
    /// </summary>
    public RepeatTypeEnum RepeatType { get; set; }

    public string GetRepeatTypeValue()
    {
      return ((int)RepeatType).ToString();
    }

    /// <summary>
    /// Se for semanalmente, o padrão de dias da semana deve ser informado.
    /// O horário buscará do valor "Time"
    /// </summary>
    public RepeatWeek RepeatWeekPattern { get; set; }

    /// <summary>
    /// Se for mensalmente, o padrão de dias do mês deve ser informado.
    /// Nesse caso deve ser informado o dia de cada mês que executará.
    /// </summary>
    public RepeatMonth RepeatMonthPattern { get; set; }

    /// <summary>
    /// Hora de execução em caso de agendamento diário, repetindo de hora em hora informada.
    /// Ex: Se for informado "04:00" de 4 em 4 horas o processo será executado todos os dias.
    /// </summary>
    public string RepeatTime { get; set; }
  }
  public class RepeatWeek
  {
    /// <summary>
    /// Será executado no domingo?
    /// </summary>
    public bool Sunday { get; set; }

    /// <summary>
    /// Será executado na segunda-feira?
    /// </summary>
    public bool Monday { get; set; }

    /// <summary>
    /// Será executado na terça-feira?
    /// </summary>
    public bool Tuesday { get; set; }

    /// <summary>
    /// Será executado na quarta-feira?
    /// </summary>
    public bool Wednesday { get; set; }

    /// <summary>
    /// Será executado na quinta-feira?
    /// </summary>
    public bool Thursday { get; set; }

    /// <summary>
    /// Será executado na sexta-feira?
    /// </summary>
    public bool Friday { get; set; }

    /// <summary>
    /// Será executado no sábado?
    /// </summary>
    public bool Saturday { get; set; }
  }
  /// <summary>
  /// Opções de repetição mensal.
  /// </summary>
  public class RepeatMonth
  {
    /// <summary>
    /// Dia que será executdo.
    /// </summary>
    public int Day { get; set; }

    /// <summary>
    /// Ultimo dia do mês será executado.
    /// </summary>
    public bool Last { get; set; }
  }
  /// <summary>
  /// Opções de schedule
  /// </summary>
  public enum TypeEnum
  {
    /// <summary>
    /// Será executado imediatamente
    /// </summary>
    tNow = 0,

    /// <summary>
    /// Será executado em uma data específica a ser informada.
    /// </summary>
    tDate = 1,
  }
  /// <summary>
  /// Tipo de opões de Schedule
  /// </summary>
  public enum RepeatTypeEnum
  {
    /// <summary>
    /// Cliclos acontecem diariamente
    /// </summary>
    rtDaily = 0,

    /// <summary>
    /// Cliclos ocorrem semanalmente
    /// </summary>
    rtWeekly = 1,

    /// <summary>
    /// Ciclos ocorrem mensalmente
    /// </summary>
    rtMonthly = 2
  }
  /// <summary>
  /// Parametros de geração do relatóiro.
  /// </summary>
  public class GenerateParams
  {
    /// <summary>
    /// Lista de parâmetros do relatório
    /// </summary>
    public IEnumerable<ParameterParam> Parameters { get; set; }

    /// <summary>
    /// Lista de filtros do relatório
    /// </summary>
    public IEnumerable<FilterParam> Filters { get; set; }

    /// <summary>
    /// Identifica se o relatório é apenas para visualização. Caso verdadeiro não será 
    /// gerado pelo job (não será criado um agendamento).
    /// </summary>
    public bool IsView { get; set; } = false;

    /// <summary>
    /// Para a execução do relatório em caso de erro?
    /// </summary>
    public bool StopExecutionOnError
    {
      get { return _StopExecutionOnError; }
      set { _StopExecutionOnError = value; }
    }
    private bool _StopExecutionOnError = true;

    /// <summary>
    /// Tipo de arquivo gerado após a execução do relatóiro.
    ///  PDF = 0, NONE = 1, CSV = 2, JPEG = 3, HTML = 4, XLSX = 5, MHT = 6, RTF = 7, TXT = 8, XLS = 9
    /// </summary>
    public ExportFileTypeEnum ExportFileType
    {
      get { return _ExportFileType; }
      set { _ExportFileType = value; }
    }
    private ExportFileTypeEnum _ExportFileType = ExportFileTypeEnum.PDF;

    public string LicenseContext { get; set; }

    public string CustomData { get; set; }
  }

  /// <summary>
  /// Parametros do relatório
  /// </summary>


  public class ParameterParam
  {
    /// <summary>
    /// Nome do parâmetro
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Valor do parâmetro
    /// </summary>
    public string Value { get; set; }
    /// <summary>
    /// Tipo do parâmetro
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// Descrição do parâmetro
    /// </summary>
    public string Description { get; set; }
  }

  public class FilterParam
  {
    /// <summary>
    /// Nome da tabela em que o filtro será aplicado
    /// </summary>
    public string TableName { get; set; }
    /// <summary>
    /// Condições do filtro
    /// </summary>
    public List<FilterItemParam> FilterItens { get; set; }
    /// <summary>
    /// Indica se o filtro está ativo ou não
    /// </summary>
    public bool Active { get; set; }
  }

  public class FilterItemParam
  {
    /// <summary>
    /// E/OU (E - Todas as condições são respeitadas, OU - No mínimo uma condição é respeitada)
    /// </summary>
    public string AndOr { get; set; }
    /// <summary>
    /// Parênteses para abrir a condição 
    /// </summary>
    public string OpenParenth { get; set; }
    /// <summary>
    /// Nome da conluna em que o filtro será aplicado
    /// </summary>
    public string Column { get; set; }
    /// <summary>
    /// Tipo da coluna
    /// </summary>
    public string DataType { get; set; }
    /// <summary>
    /// Operador da condição do filtro (ex: =, ISNULL, LIKE)
    /// </summary>
    public string Operator { get; set; }
    /// <summary>
    /// Valor do filtro
    /// </summary>
    public string Value { get; set; }
    /// <summary>
    /// Parênteses para fechar a condição 
    /// </summary>
    public string CloseParenth { get; set; }
  }

  /// <summary>
  /// Tipo de exportação do relatório
  /// </summary>

  public enum ExportFileTypeEnum
  {
    PDF = 0,
    NONE = 1,
    CSV = 2,
    JPEG = 3,
    HTML = 4,
    XLSX = 5,
    MHT = 6,
    RTF = 7,
    TXT = 8,
    XLS = 9,
    DOCX = 10,
  }


  /// <summary>
  /// Informações contendo a Promessa de solicitação de geração de relatórios.
  /// </summary>
  public class GenerateReportReturnDto
  {
    /// <summary>
    /// Identificador universal da solicitação de geração do relatório.
    /// </summary>
    public string UIdRequest { get; set; }

    /// <summary>
    /// Próxima de data de execução.
    /// </summary>
    public DateTimeOffset ScheduleDate { get; set; }


  }

  /// <summary>
  /// Dados de relatórios gerados
  /// </summary>
  public class ExecutionScheduleDto
  {
    public int Id { get; set; }
    /// <summary>
    /// Identificador de solicitação de geração de relatório.
    /// </summary>
    public int RequestId { get; set; }

    /// <summary>
    /// Data/hora agendada para execução
    /// </summary>
    public DateTimeOffset ScheduleDate { get; set; }

    /// <summary>
    /// Servidor que realizou a execução.
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// Data inicio da execução
    /// </summary>
    public DateTimeOffset? StartDate { get; set; }

    /// <summary>
    /// Data final da execução
    /// </summary>
    public DateTimeOffset? EndDate { get; set; }

    /// <summary>
    /// Status da execução do agendamento.
    /// </summary>
    //public ExecutionScheduleStatusValues Status { get; set; }
    public string Status { get; set; }

    /// <summary>
    /// Memória de execução
    /// </summary>
    public string ExecutionMemory { get; set; }

    /// <summary>
    /// Mensagem de erro da execução do job.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTimeOffset CreatedOn { get; set; }

    /// <summary>
    /// Data de modificação
    /// </summary>
    public DateTimeOffset? ModifiedOn { get; set; }

    /// <summary>
    /// Usuario de criação
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Usuario de modificação
    /// </summary>
    public string ModifiedBy { get; set; }
  }
  /// <summary>
  /// Status do processamento da execução do relatório.
  /// </summary>
  public enum ExecutionScheduleStatusValues
  {
    /// <summary>
    /// O Job ainda não foi executado e está pendente
    /// </summary>
    Pending = 0,
    /// <summary>
    /// O Job está em execução
    /// </summary>
    Running = 1,
    /// <summary>
    /// O Job terminou normalmente a sua execução
    /// </summary>
    Finished = 2,
    /// <summary>
    /// A execução do Job foi cancelada
    /// </summary>
    Canceled = 3,
    /// <summary>
    /// A execução do Job começou e foi interrompida
    /// </summary>
    Stopped = 4,
    /// <summary>
    /// Erro na execução do job
    /// </summary>
    Error = 5,
    /// <summary>
    /// O Job foi executado com avisos
    /// </summary>
    Warning = 6,
    /// <summary>
    /// Houve uma falha no servidor durante a execução do Job e a mesma não terminou corretamente
    /// ou no momento da execução do Job, o servidor não estava ativo
    /// </summary>
    Failed = 7,
    /// <summary>
    /// A execução do job está suspensa até ele ser habilitado novamente
    /// Se ele for habilitado antes da sua hora programada ele roda normalmente, se for
    /// depois da sua hora programada e ele tiver recorrência, sua recorrência é recalculada e
    /// ELE é atualizado para aquela data/hora recalculada; se ele não tiver recorrência, seu
    /// status vira Canceled.
    /// </summary>
    Disabled = 8
  }
}

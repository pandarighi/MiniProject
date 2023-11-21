namespace TestApp.Models;

public class Stage
{
    public string StageId { get; set; }
    public string StageDesc { get; set; }
}
public class ResponseStage
{
    public List<Stage> Data { get; set; }
}

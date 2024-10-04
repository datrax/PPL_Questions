public class QuestionLine
{
    public string RowNum { get; set; }
    public string QuestionCode { get; set; }
    public string Question { get; set; }
    public string Answer1 { get; set; }
    public string Answer2 { get; set; }
    public string Answer3 { get; set; }
    public string Answer4 { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        var qLine = (obj as QuestionLine)!;
        return Question == qLine.Question && Answer1 == qLine.Answer1 && Answer2 == qLine.Answer2 && Answer3 == qLine.Answer3 && Answer4 == qLine.Answer4;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Question, Answer1, Answer2, Answer3, Answer4);
    }
}

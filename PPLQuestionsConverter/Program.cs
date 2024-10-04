class Program
{
    public static void Main(string[] args)
    {
        string inputFilePath = @"ppla_eng.pdf";
        string outputFilePath = "../../../questions.json";

        var questions = new List<QuestionLine>();

        if (File.Exists(outputFilePath))
        {
            questions = FileHelper.GetFromOutputFile<List<QuestionLine>>(outputFilePath);
        }
        else
        {

            var files = PdfHelper.SplitPdfIntoChunks(inputFilePath, 10);

            foreach (var file in files)
            {
                PdfHelper.AddQuestionsFromFile(questions, file);
            }

            //remove header line
            questions.RemoveAt(0);

            FileHelper.SaveToOutput(outputFilePath, questions);

            Console.WriteLine("Successfully converted");
        }

        //remove duplicates
        questions = questions!
                    .GroupBy(d => d)
                    .Select(g => g.First())
                    .ToList();

        FileHelper.SaveToOutput(outputFilePath, questions);

        Console.WriteLine("Removed duplicates");

    }
}

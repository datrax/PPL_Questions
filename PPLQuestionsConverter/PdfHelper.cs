using iTextSharp.text;
using iTextSharp.text.pdf;
using Spire.Pdf.Utilities;


public static class PdfHelper
{
    public static List<byte[]> SplitPdfIntoChunks(string inputFilePath, int chunkSize)
    {
        List<byte[]> pdfChunks = new List<byte[]>();

        using (PdfReader reader = new PdfReader(inputFilePath))
        {
            int totalPages = reader.NumberOfPages;

            for (int pageIndex = 1; pageIndex <= totalPages; pageIndex += chunkSize)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document();
                    PdfCopy copy = new PdfCopy(document, ms);
                    document.Open();

                    for (int i = 0; i < chunkSize && (pageIndex + i) <= totalPages; i++)
                    {
                        PdfImportedPage page = copy.GetImportedPage(reader, pageIndex + i);
                        copy.AddPage(page);
                    }

                    document.Close();

                    pdfChunks.Add(ms.ToArray());
                }
            }
        }

        return pdfChunks;
    }

    public static List<QuestionLine> AddQuestionsFromFile(List<QuestionLine> questions, byte[] bytes)
    {
        Spire.Pdf.PdfDocument pdf = new();
        pdf.LoadFromBytes(bytes);
        PdfTableExtractor extractor = new PdfTableExtractor(pdf);

        for (int i = 0; i < pdf.Pages.Count; i++)
        {
            PdfTable[] tables = extractor.ExtractTable(i);
            if (tables == null)
            {
                Console.WriteLine($"Page {i} is empty");
                continue;
            }
            var pdfTable = tables.First();

            int rowIndex = 0;
            if (string.IsNullOrEmpty(pdfTable.GetText(0, 0)))
            {
                var lastItem = questions.Last();
                lastItem.Question += pdfTable.GetText(0, 2);
                lastItem.Answer1 += pdfTable.GetText(0, 3);
                lastItem.Answer2 += pdfTable.GetText(0, 4);
                lastItem.Answer3 += pdfTable.GetText(0, 5);
                lastItem.Answer4 += pdfTable.GetText(0, 6);
                rowIndex = 1;
            }

            for (int r = rowIndex; r < pdfTable.GetRowCount(); r++)
            {
                QuestionLine question = new()
                {
                    RowNum = pdfTable.GetText(r, 0),
                    QuestionCode = pdfTable.GetText(r, 1),
                    Question = pdfTable.GetText(r, 2),
                    Answer1 = pdfTable.GetText(r, 3),
                    Answer2 = pdfTable.GetText(r, 4),
                    Answer3 = pdfTable.GetText(r, 5),
                    Answer4 = pdfTable.GetText(r, 6),
                };

                questions.Add(question);
            }
        }

        return questions;
    }
}

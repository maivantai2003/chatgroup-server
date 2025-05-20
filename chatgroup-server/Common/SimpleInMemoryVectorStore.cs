using LangChain.Providers.OpenAI;
using OpenAI.Embeddings;

namespace chatgroup_server.Common
{
    public class SimpleInMemoryVectorStore
    {
        //private readonly List<(string Text, float[] Embedding)> _data = new();
        //private readonly OpenAiEmbedding _embedder;

        //public SimpleInMemoryVectorStore(string apiKey)
        //{
        //    _embedder = new OpenAiEmbedding(apiKey);
        //}

        //public async Task AddAsync(IEnumerable<string> texts)
        //{
        //    foreach (var text in texts)
        //    {
        //        var embedding = await _embedder.CallAsync(text);
        //        _data.Add((text, embedding));
        //    }
        //}

        //public async Task<List<string>> SimilarAsync(string input, int topK = 3)
        //{
        //    var inputEmbedding = await _embedder.CallAsync(input);

        //    return _data
        //        .Select(d => new { d.Text, Score = CosineSimilarity(d.Embedding, inputEmbedding) })
        //        .OrderByDescending(x => x.Score)
        //        .Take(topK)
        //        .Select(x => x.Text)
        //        .ToList();
        //}

        //private float CosineSimilarity(float[] a, float[] b)
        //{
        //    var dot = a.Zip(b, (x, y) => x * y).Sum();
        //    var normA = Math.Sqrt(a.Sum(x => x * x));
        //    var normB = Math.Sqrt(b.Sum(x => x * x));
        //    return (float)(dot / (normA * normB));
        //}
    }
}

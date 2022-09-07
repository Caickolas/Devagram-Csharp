using Devagram_Csharp.Dtos;
using System.Net.Http.Headers;

namespace Devagram_Csharp.Services
{
    public class CosmicService
    {
        public string EnviarImagem(ImagemDTO imagemdto)
        {
            Stream imagem = imagemdto.Imagem.OpenReadStream();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "31SpigX4d4dESVecnN9iBIg8gzSIoBYE3rHXRuUi93nyNQ1JMl");

            var request = new HttpRequestMessage(HttpMethod.Post, "file");
            var conteudo = new MultipartFormDataContent
            {
                { new StreamContent(imagem),"media", imagemdto.Nome }
            };

            request.Content = conteudo;

            var retornoreq = client.PostAsync("https://upload.cosmicjs.com/v2/buckets/devagram-devagram/media", request.Content).Result;

            var urlretorno = retornoreq.Content.ReadFromJsonAsync<CosmicRespostaDTO>();

            return urlretorno.Result.media.url;
        }

    }
}

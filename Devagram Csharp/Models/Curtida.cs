using System.ComponentModel.DataAnnotations.Schema;

namespace Devagram_Csharp.Models
{
    public class Curtida
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdPublicacao { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario usuario { get; private set; }

        [ForeignKey("IdPublicacao")]
        public virtual Publicacao publicacao { get; private set; }
    }
}

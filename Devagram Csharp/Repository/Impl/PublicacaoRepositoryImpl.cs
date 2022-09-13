﻿using Devagram_Csharp.Models;

namespace Devagram_Csharp.Repository.Impl
{
    public class PublicacaoRepositoryImpl : IPublicacaoRepository
    {
        private readonly DevagramContext _context;

        public PublicacaoRepositoryImpl(DevagramContext context)
        {
            _context = context;
        }
        public void Publicar(Publicacao publicacao)
        {
            _context.Add(publicacao);
            _context.SaveChanges();
        }
    }
}
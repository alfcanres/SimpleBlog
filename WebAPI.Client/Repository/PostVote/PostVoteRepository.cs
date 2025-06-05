using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using WebAPI.Client.Helpers;

using WebAPI.Client.ViewModels;

namespace WebAPI.Client.Repository.PostVote
{
    public class PostVoteRepository : BaseRepository<PostVoteCreateDTO, PostVoteResultDTO, PostVoteUpdateDTO>, IPostVoteRepository
    {
        public PostVoteRepository(IHttpClientHelper httpClientHelper)
            : base("api/postvote", httpClientHelper)
        {
            
        }

    }
}

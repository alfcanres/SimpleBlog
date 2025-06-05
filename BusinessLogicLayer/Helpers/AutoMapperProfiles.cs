using AutoMapper;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;

namespace BusinessLogicLayer.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<PostTypeCreateDTO, PostType>();
            CreateMap<PostTypeUpdateDTO, PostType>();
            CreateMap<PostType, PostTypeReadDTO>()
                 .ForMember(t => t.PostCount, t => t.MapFrom(x => x.Posts.Count))
                 .ReverseMap();

            CreateMap<MoodTypeCreateDTO, MoodType>();
            CreateMap<MoodTypeUpdateDTO, MoodType>();
            CreateMap<MoodType, MoodTypeReadDTO>()
                 .ForMember(t => t.PostCount, t => t.MapFrom(x => x.Posts.Count))
                 .ReverseMap();

            CreateMap<PostCreateDTO, Post>();
            CreateMap<PostUpdateDTO, Post>();
            CreateMap<Post, PostReadDTO>()
                .ForMember(t => t.UserName, t => t.MapFrom(m => m.ApplicationUserInfo.UserName))
                .ForMember(t => t.UserFullName, t => t.MapFrom(m => m.ApplicationUserInfo.FirstName + " " + m.ApplicationUserInfo.LastName))
                .ForMember(t => t.ProfilePic, t => t.MapFrom(m => m.ApplicationUserInfo.ProfilePicture))
                .ForMember(t => t.PostType, t => t.MapFrom(m => m.PostType.Description))
                .ForMember(t => t.MoodType, t => t.MapFrom(m => m.MoodType.Mood))
                .ReverseMap();

            CreateMap<Post, PostPendingToPublishDTO>()
                .ForMember(t => t.UserName, t => t.MapFrom(m => m.ApplicationUserInfo.UserName))
                .ForMember(t => t.ProfilePic, t => t.MapFrom(m => m.ApplicationUserInfo.ProfilePicture))
                .ForMember(t => t.PostType, t => t.MapFrom(m => m.PostType.Description))
                .ForMember(t => t.MoodType, t => t.MapFrom(m => m.MoodType.Mood));

            CreateMap<Post, PostListDTO>()
                .ForMember(t => t.UserName, t => t.MapFrom(m => m.ApplicationUserInfo.UserName))
                .ForMember(t => t.ProfilePic, t => t.MapFrom(m => m.ApplicationUserInfo.ProfilePicture))
                .ForMember(t => t.PostType, t => t.MapFrom(m => m.PostType.Description))
                .ForMember(t => t.MoodType, t => t.MapFrom(m => m.MoodType.Mood))
                .ForMember(t => t.Votes, t => t.MapFrom(m => m.Votes.Count()))
                .ForMember(t => t.Comments, t => t.MapFrom(m => m.Comments.Count()));


            CreateMap<ApplicationUserInfoCreateDTO, ApplicationUserInfo>();
            CreateMap<ApplicationUserInfoUpdateDTO, ApplicationUserInfo>();
            CreateMap<ApplicationUserInfo, ApplicationUserInfoReadDTO>()
                .ReverseMap();


            CreateMap<ApplicationUserInfo, ApplicationUserInfoListDTO>()
                .ForMember(t => t.FullName, t => t.MapFrom(m => m.FirstName + " " + m.LastName))
                .ForMember(t => t.PostsCount, t => t.MapFrom(m => m.Posts.Count))
                .ForMember(t => t.PostsPedingForPublish, t => t.MapFrom(m => m.Posts.Where(w => !w.IsPublished).Count()))
                .ForMember(t => t.CommentsCount, t => t.MapFrom(m => m.Comments.Count));


            CreateMap<PostCommentCreateDTO, PostComment>();
            CreateMap<PostCommentUpdateDTO, PostComment>();
            CreateMap<PostComment, PostCommentReadDTO>()
                .ForMember(t => t.ProfilePicture, t => t.MapFrom(m => m.ApplicationUserInfo.ProfilePicture))
                .ForMember(t => t.UserName, t => t.MapFrom(m => m.ApplicationUserInfo.UserName))
                .ReverseMap();

        }
    }
}

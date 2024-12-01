using AutoMapper;
using HMS_Final.Controllers.Feedbacks.DTO;
using HMS_Final.Models;
using System.Runtime;

namespace HMS_Final.Controllers.Feedbacks
{
    public class FeedbackAutoMapper : Profile
    {
        public FeedbackAutoMapper()
        {
            CreateMap<FeedbackCreateDTO, Feedback>().ReverseMap();
            CreateMap<FeedbackUpdateDTO, Feedback>().ReverseMap();
            CreateMap<FeedbackGetByIdDTO, Feedback>().ReverseMap();
            CreateMap<FeedbackGetDTO, Feedback>().ReverseMap();
            CreateMap<FeedbackDeleteDTO, Feedback>().ReverseMap();
        }
    }

}

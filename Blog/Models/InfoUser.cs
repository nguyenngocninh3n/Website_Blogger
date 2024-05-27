using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;

namespace Blog.Models
{
    public class Dates
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
    public class InfoUser
    {
        public string UserName { get; set; }
        public Dates Birthday { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Degree { get; set; }
        public string Job { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string LinkedIn { get; set; }
        


        public bool UserNameMode { get; set; }
        public bool BirthdayMode { get; set; }
        public bool SexMode { get; set; }
        public bool EmailMode { get; set; }
        public bool AddressMode { get; set; }
        public bool DegreeMode { get; set; }
        public bool JobMode { get; set; }
        public bool FacebookMode { get; set; }
        public bool InstagramMode { get; set; }
        public bool YoutubeMode { get; set; }
        public bool LinkedInMode { get; set; }

      
        public InfoUser(string userName, Dates birthday, string sex, string email, string address, string degree, string job, string facebook, string instagram, string youtube, string linkedIn,  bool birthdayMode, bool sexMode, bool emailMode, bool addressMode, bool degreeMode, bool jobMode, bool facebookMode, bool instagramMode, bool youtubeMode, bool linkedInMode)
        {
            UserName = userName;
            Birthday = birthday;
            Sex = sex;
            Email = email;
            Address = address;
            Degree = degree;
            Job = job;
            Facebook = facebook;
            Instagram = instagram;
            Youtube = youtube;
            LinkedIn = linkedIn;
            
      
            BirthdayMode = birthdayMode;
            SexMode = sexMode;
            EmailMode = emailMode;
            AddressMode = addressMode;
            DegreeMode = degreeMode;
            JobMode = jobMode;
            FacebookMode = facebookMode;
            InstagramMode = instagramMode;
            YoutubeMode = youtubeMode;
            LinkedInMode = linkedInMode;
        }

        public InfoUser(User user, User_mode usermode)
        {
            UserName = user.UserName;
            Birthday.Day = user.Birthday.Day;
            Birthday.Month = user.Birthday.Month;
            Birthday.Year = user.Birthday.Year;
            Sex = user.Sex;
            Email = user.Email;
            Address = user.Address;
            Degree = user.Degree;
            Job = user.Job;
            Facebook = user.Facebook;
            Instagram = user.Instagram;
            Youtube = user.Youtube;
            LinkedIn = user.LinkedIn;


            BirthdayMode = usermode.BirthdayMode;
            SexMode = usermode.SexMode;
            EmailMode = usermode.EmailMode;
            AddressMode = usermode.AddressMode;
            DegreeMode = usermode.DegreeMode;
            JobMode = usermode.JobMode;
            FacebookMode = usermode.FacebookMode;
            InstagramMode = usermode.InstagramMode;
            YoutubeMode = usermode.YoutubeMode;
            LinkedInMode = usermode.LinkedInMode;
        }
    }
}
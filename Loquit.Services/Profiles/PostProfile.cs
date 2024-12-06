using AutoMapper;
using Loquit.Data.Entities;
using Loquit.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loquit.Services.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDTO>()
                .ReverseMap();
        }
    }
}

/*
 * Categories:
-food           id:0
-music          id:1
-art            id:2
-nature         id:3
-pets           id:4
-videogames     id:5
-culture        id:6
-funny          id:7
-factology      id:8


Tags:
-spoiler
-nsfw


Evaluations:
-politics         0 neutral --- biased 1
-mood             0 sad     --- happy  1
-eval_3           0 calm    --- a lot  1
-brainrot         0 low     --- tiktok 1
-controversial    0 safe    --- a lot  1


Preferences:
category modifier 0 - 1
default 0.30

food - 0.30
music - 0.30
...

on click + 0.10
every day 0.05 towards default
on like + 0.05
on dislike - 0.10


Criteria:
default 1
+ category modifier
+ for each eval score * specific preference
if (dont show nsfw) score * 0
*/

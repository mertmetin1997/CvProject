﻿using Core.Business;
using cvProject.Entity.Concrete;
using cvProject.Entity.Dtos.About;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvProject.Business.Abstract
{
    public interface IAboutService : IGenericService<About,AboutResponseDto,AboutCreateRequestDto,AboutUpdateRequestDto,AboutDetailResponseDto>
    {
    }
}

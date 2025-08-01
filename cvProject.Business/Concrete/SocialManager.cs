﻿using AutoMapper;
using Core.UnitOfWorks;
using Core.Utilities.Result;
using cvProject.Business.Abstract;
using cvProject.Business.Constans;
using cvProject.DataAccess.Abstract;
using cvProject.Entity.Concrete;
using cvProject.Entity.Dtos.SocialAccount;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvProject.Business.Concrete
{
    public class SocialManager : ISocialAccountService
    {
        private readonly ISocialAccountRepository _socialAccountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public SocialManager(ISocialAccountRepository socialAccountRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _socialAccountRepository = socialAccountRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IDataResult<SocialAccountResponseDto>> AddAsync(SocialAccountCreateRequestDto dto)
        {
            try
            {
                var social = _mapper.Map<SocialAccount>(dto);
                await _socialAccountRepository.AddAsync(social);
                await _unitOfWork.CommitAsync();
                var response = _mapper.Map<SocialAccountResponseDto>(social);
                return new SuccessDataResult<SocialAccountResponseDto>(response, ResultMessages.SuccessCreated);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<SocialAccountResponseDto>(e.Message);
            }
        }

        public async Task<IDataResult<IEnumerable<SocialAccountResponseDto>>> GetAllAsync()
        {
            try
            {
                var socials = await _socialAccountRepository.GetAll(s => !s.IsDeleted).ToListAsync();
                if (socials == null)
                {
                    return new ErrorDataResult<IEnumerable<SocialAccountResponseDto>>(ResultMessages.ErrorListed);
                }
                var dtos = _mapper.Map<IEnumerable<SocialAccountResponseDto>>(socials);
                return new SuccessDataResult<IEnumerable<SocialAccountResponseDto>>(dtos, ResultMessages.SuccessListed);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<IEnumerable<SocialAccountResponseDto>>(e.Message);
            }
        }

        public async Task<IDataResult<SocialAccountResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var social = await _socialAccountRepository.GetAsync(s => s.Id == id );
                if (social == null)
                {
                    return new ErrorDataResult<SocialAccountResponseDto>(ResultMessages.ErrorGet);
                }
                var response = _mapper.Map<SocialAccountResponseDto>(social);
                return new SuccessDataResult<SocialAccountResponseDto>(response, ResultMessages.SuccessGet);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<SocialAccountResponseDto>(e.Message);
            }
        }

        public async Task<IDataResult<SocialAccountResponseDto>> GetSocialAccountByNameAsync(string platform)
        {
            try
            {
                var social = await _socialAccountRepository.GetAsync(s => s.Name == platform);
                if (social == null)
                {
                    return new ErrorDataResult<SocialAccountResponseDto>(ResultMessages.ErrorGet);
                }

                var response = _mapper.Map<SocialAccountResponseDto>(social);
                return new SuccessDataResult<SocialAccountResponseDto>(response, ResultMessages.SuccessGet);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<SocialAccountResponseDto>(e.Message);
            }
        }

        public async Task<IDataResult<IEnumerable<SocialAccountResponseDto>>> GetSocialAccountByUserNameAsync(string username)
        {
            try
            {
                var socials = await _socialAccountRepository.GetAll(s => s.UserName == username).ToListAsync();
                if (socials == null)
                {
                    return new ErrorDataResult<IEnumerable<SocialAccountResponseDto>>(ResultMessages.ErrorListed);
                }

                var response = _mapper.Map<IEnumerable<SocialAccountResponseDto>>(socials);
                return new SuccessDataResult<IEnumerable<SocialAccountResponseDto>>(response, ResultMessages.SuccessListed);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<IEnumerable<SocialAccountResponseDto>>(e.Message);
            }
        }

        public async Task<IResult> RemoveAsync(Guid id)
        {
            try
            {
                var social = await _socialAccountRepository.GetAsync(s => s.Id == id);
                if (social == null)
                {
                        return new ErrorResult(ResultMessages.ErrorGet);
                }
                social.UpdateAt = DateTime.Now;
                social.IsDeleted = true;
                social.IsActive = false;
                _socialAccountRepository.Update(social);
                await _unitOfWork.CommitAsync();
                return new SuccessResult(ResultMessages.SuccessDeleted);
            }
            catch (Exception e) 
            {

                return new ErrorResult(e.Message);
            }
        }

        public async Task<IResult> UpdateAsync(SocialAccountUpdateRequestDto dto)
        {
            try
            {
                var social = _mapper.Map<SocialAccount>(dto);
                social.UpdateAt = DateTime.Now;
                _socialAccountRepository.Update(social);
                await _unitOfWork.CommitAsync();
                return new SuccessResult(ResultMessages.SuccessUpdated);
            }
            catch (Exception e)
            {

                return new ErrorResult(e.Message);
            }
        }
    }
}

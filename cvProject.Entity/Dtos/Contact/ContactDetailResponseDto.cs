﻿using Core.Entities;

namespace cvProject.Entity.Dtos.Contact
{
    public sealed record ContactDetailResponseDto(
        Guid Id,
        string Address,
        string City,
        string Town,
        string Phone,
        string Email,
        bool IsActive,
        bool IsDeleted
        ) : IDetailDto;

}

﻿using Core.Entities;

namespace Api.Dtos
{
    public class UserInfoDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public AddressDto Address { get; set; }
        public List<TaxReturnDtoReponse> taxReturnDtoReponses { get; set; }
    }
}

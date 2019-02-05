﻿using Agri.Data.Migrations;
using Agri.Models;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using AutoMapper;
using SERVERAPI.ViewModels;

namespace SERVERAPI
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
            : this("MyProfile")
        {
        }
        protected AutoMapperProfileConfiguration(string profileName)
            : base(profileName)
        {
            CreateMap<AmmoniaRetention, AmmoniaRetention>();
            CreateMap<ManureStorageSystem, ManureStorageSystem>();
            CreateMap<ManureImportedDetailViewModel, ImportedManure>()
                .ForMember(dest => dest.Id, x => x.MapFrom(src => src.ManureImportId))
                .ForMember(dest => dest.ManureType, x => x.MapFrom(src => src.SelectedManureType))
                .ForMember(dest => dest.ManureTypeName, x => x.MapFrom(src => EnumHelper<ManureMaterialType>.GetDisplayValue(src.SelectedManureType)))
                .ForMember(dest => dest.Units, x => x.MapFrom(src => src.SelectedAnnualAmountUnit))
                .ReverseMap();
            CreateMap<ImportedManure, ImportedManure>();
            CreateMap<UserPrompt, UserPrompt>();
            CreateMap<AnimalSubType, AnimalSubType>();
            CreateMap<SeparatedSolidManure, SeparatedSolidManure>();
            CreateMap<SubRegions, SubRegions>();
            CreateMap<PreviousYearManureApplicationNitrogenDefault, PreviousYearManureApplicationNitrogenDefault>();

            CreateMap<StaticDataVersion, StaticDataVersion>();
        }
    }
}

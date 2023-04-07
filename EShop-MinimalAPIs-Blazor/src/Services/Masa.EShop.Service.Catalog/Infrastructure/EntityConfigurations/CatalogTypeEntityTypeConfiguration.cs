﻿using Masa.EShop.Service.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masa.EShop.Service.Catalog.Infrastructure.EntityConfigurations;

class CatalogTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable(nameof(CatalogType));
        
        builder.HasKey(ct => ct.Id);

        builder.Property(ct => ct.Id)
           .IsRequired();

        builder.Property(ct => ct.Type)
            .IsRequired()
            .HasMaxLength(100);
    }
}
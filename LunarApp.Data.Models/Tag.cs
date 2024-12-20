﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using static LunarApp.Common.ValidationConstants.Tag;

namespace LunarApp.Data.Models
{
    public class Tag
    {
        [Key]
        [Comment("Tag Identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(TagNameMaxLength)]
        [Comment("Tag name")]
        public required string Name { get; set; }
        [Required]
        [Comment("User Identifier")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}

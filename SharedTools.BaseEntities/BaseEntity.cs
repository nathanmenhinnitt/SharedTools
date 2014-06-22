﻿namespace SharedTools.BaseEntities
{
    using System;

    public class BaseEntity
    {
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public int? CreatedByUserId { get; set; }

        public DateTime? LastUpdated { get; set; }

        public int? LastUpdatedByUserId { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int? DeletedByUserId { get; set; }

        public bool IsActive { get; set; }
    }
}

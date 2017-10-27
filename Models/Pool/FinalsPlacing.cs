using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Pool
{
  public class FinalsPlacing : Entity {
    // [Required, ForeignKey("CountryId")]
    // public virtual Country Country { get; set; }

    [Required]
		public int CountryId { get; set; }

    // [ForeignKey("FinalsId")]
    // public virtual Finals Finals { get; set; }

    public int? FinalsId { get; set; }
  }
}

/*
CREATE TABLE [dbo].[FinalsPlacing](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Country] [int] NOT NULL,
	[Final] [int] NULL,
 CONSTRAINT [PK_FinalsPlacing] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 */

/*
ALTER TABLE [dbo].[FinalsPlacing]  WITH CHECK ADD  CONSTRAINT [FK_FinalsPlacing_Country] FOREIGN KEY([Country]) REFERENCES [dbo].[Country] ([ID])
ALTER TABLE [dbo].[FinalsPlacing]  WITH CHECK ADD  CONSTRAINT [FK_FinalsPlacing_Finals] FOREIGN KEY([Final]) REFERENCES [dbo].[Finals] ([ID])
  */
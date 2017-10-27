using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Pool
{
  public class Match : Entity {
    // [Required, ForeignKey("Country1Id")]
    // public virtual Country Country1 { get; set; }

    [Required]
    public int Country1Id { get; set; }

    // [Required, ForeignKey("Country2Id")]
    // public virtual Country Country2 { get; set; }

    [Required]
    public int Country2Id { get; set; }

    [Column(TypeName = "DateTime")]
    public DateTime StartDate { get; set; }

    [Range(0, 20)]
    public int GoalsCountry1 { get; set; }

    [Range(0, 20)]
    public int GoalsCountry2 { get; set; }

    [MaxLength(50), RegularExpression("^[^<>]+$")]
    public string Location { get; set; }
  }
}

/*
CREATE TABLE [dbo].[Match](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Country1] [int] NOT NULL,
	[Country2] [int] NOT NULL,
	[Start] [datetime] NULL,
	[GoalsCountry1] [int] NULL,
	[GoalsCountry2] [int] NULL,
	[Location] [nvarchar](50) NULL,
 CONSTRAINT [PK_Match] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 */

/*
=ALTER TABLE [dbo].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_Country] FOREIGN KEY([Country1]) REFERENCES [dbo].[Country] ([ID])
ALTER TABLE [dbo].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_Country1] FOREIGN KEY([Country2])
  */
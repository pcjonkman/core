using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Pool
{
  public class MatchFinals : Entity {
    // [ForeignKey("Country1Id")]
    // public virtual Country Country1 { get; set; }

    public int? Country1Id { get; set; }

    // [ForeignKey("Country2Id")]
    // public virtual Country Country2 { get; set; }

    public int? Country2Id { get; set; }

    [Column(TypeName = "DateTime")]
    public DateTime StartDate { get; set; }

    [Range(0, 20)]
    public int GoalsCountry1 { get; set; }

    [Range(0, 20)]
    public int GoalsCountry2 { get; set; }

    [MaxLength(50), RegularExpression("^[^<>]+$")]
    public string Location { get; set; }

    [MaxLength(50), RegularExpression("^[^<>]+$")]
    public string Country1Text { get; set; }

    [MaxLength(50), RegularExpression("^[^<>]+$")]
    public string Country2Text { get; set; }

    [ForeignKey("LevelNumber")]
    public virtual Finals Finals { get; set; }

    public int? LevelNumber { get; set; }
  }
}

/*
CREATE TABLE [dbo].[MatchFinals](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Country1] [int] NULL,
	[Country2] [int] NULL,
	[Start] [datetime] NULL,
	[GoalsCountry1] [int] NULL,
	[GoalsCountry2] [int] NULL,
	[Location] [nvarchar](50) NULL,
	[Country1Text] [nvarchar](50) NULL,
	[Country2Text] [nvarchar](50) NULL,
	[LevelNumber] [int] NULL,
 CONSTRAINT [PK_MatchFinals] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 */

/*
ALTER TABLE [dbo].[MatchFinals]  WITH CHECK ADD  CONSTRAINT [FK_MatchFinals_Country] FOREIGN KEY([Country1]) REFERENCES [dbo].[Country] ([ID])
ALTER TABLE [dbo].[MatchFinals]  WITH CHECK ADD  CONSTRAINT [FK_MatchFinals_Country1] FOREIGN KEY([Country2]) REFERENCES [dbo].[Country] ([ID])
ALTER TABLE [dbo].[MatchFinals]  WITH CHECK ADD  CONSTRAINT [FK_MatchFinals_Finals] FOREIGN KEY([LevelNumber]) REFERENCES [dbo].[Finals] ([LevelNumber])
ALTER TABLE [dbo].[MatchFinals] CHECK CONSTRAINT [FK_MatchFinals_Finals]
  */
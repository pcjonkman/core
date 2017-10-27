using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Pool
{
  public class Country : Entity {
    [Required, MaxLength(50), RegularExpression("^[^<>]+$")]
    public string Name { get; set; }

    [MaxLength(8), RegularExpression("^[^<>]+$")]
    public string Code { get; set; }

    [Required, MaxLength(1), RegularExpression("^[A-Z0-9]*$")]
    public string Group { get; set; }
  }
}

/*
CREATE TABLE [dbo].[Country](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Group] [char](1) NOT NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 */

/*
ALTER TABLE [dbo].[PoolPlayer] ADD  CONSTRAINT [DF_PoolPlayer_Score]  DEFAULT ((0)) FOR [SubScore]
ALTER TABLE [dbo].[FinalsPrediction] ADD  CONSTRAINT [DF_FinalsPrediction_SubScore]  DEFAULT ((0)) FOR [SubScore]
ALTER TABLE [dbo].[FinalsPlacing]  WITH CHECK ADD  CONSTRAINT [FK_FinalsPlacing_Country] FOREIGN KEY([Country]) REFERENCES [dbo].[Country] ([ID])
ALTER TABLE [dbo].[FinalsPlacing]  WITH CHECK ADD  CONSTRAINT [FK_FinalsPlacing_Finals] FOREIGN KEY([Final]) REFERENCES [dbo].[Finals] ([ID])
ALTER TABLE [dbo].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_Country] FOREIGN KEY([Country1]) REFERENCES [dbo].[Country] ([ID])
ALTER TABLE [dbo].[Match]  WITH CHECK ADD  CONSTRAINT [FK_Match_Country1] FOREIGN KEY([Country2])
ALTER TABLE [dbo].[PoolPlayer]  WITH CHECK ADD  CONSTRAINT [FK_PoolPlayer_aspnet_Users] FOREIGN KEY([UserID]) REFERENCES [dbo].[aspnet_Users] ([UserId])
ALTER TABLE [dbo].[PoolMessage]  WITH CHECK ADD  CONSTRAINT [FK_PoolMessage_PoolPlayer] FOREIGN KEY([PoolPlayer]) REFERENCES [dbo].[PoolPlayer] ([ID])
ALTER TABLE [dbo].[MatchPrediction]  WITH CHECK ADD  CONSTRAINT [FK_MatchPrediction_Match] FOREIGN KEY([Match]) REFERENCES [dbo].[Match] ([ID])
ALTER TABLE [dbo].[MatchPrediction]  WITH CHECK ADD  CONSTRAINT [FK_MatchPrediction_MatchPrediction] FOREIGN KEY([PoolPlayer]) REFERENCES [dbo].[PoolPlayer] ([ID])
ALTER TABLE [dbo].[FinalsPrediction]  WITH CHECK ADD  CONSTRAINT [FK_FinalsPrediction_Country] FOREIGN KEY([Country]) REFERENCES [dbo].[Country] ([ID])
ALTER TABLE [dbo].[FinalsPrediction]  WITH CHECK ADD  CONSTRAINT [FK_FinalsPrediction_Finals] FOREIGN KEY([Final]) REFERENCES [dbo].[Finals] ([ID])
ALTER TABLE [dbo].[FinalsPrediction]  WITH CHECK ADD  CONSTRAINT [FK_FinalsPrediction_PoolPlayer] FOREIGN KEY([PoolPlayer]) REFERENCES [dbo].[PoolPlayer] ([ID])
ALTER TABLE [dbo].[MatchFinals]  WITH CHECK ADD  CONSTRAINT [FK_MatchFinals_Country] FOREIGN KEY([Country1]) REFERENCES [dbo].[Country] ([ID])
ALTER TABLE [dbo].[MatchFinals]  WITH CHECK ADD  CONSTRAINT [FK_MatchFinals_Country1] FOREIGN KEY([Country2]) REFERENCES [dbo].[Country] ([ID])
ALTER TABLE [dbo].[MatchFinals]  WITH CHECK ADD  CONSTRAINT [FK_MatchFinals_Finals] FOREIGN KEY([LevelNumber]) REFERENCES [dbo].[Finals] ([LevelNumber])
ALTER TABLE [dbo].[MatchFinals] CHECK CONSTRAINT [FK_MatchFinals_Finals]
ALTER TABLE [dbo].[Finals] ADD CONSTRAINT  [UC_Finals_LevelNumber] UNIQUE ([LevelNumber])
  */
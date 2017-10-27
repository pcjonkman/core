using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Pool
{
  public class PoolPlayer : Entity {
    public PoolPlayer() {
      PoolMessages = new List<PoolMessage>();
      MatchPredictions = new List<MatchPrediction>();
      FinalsPredictions = new List<FinalsPrediction>();
    }

    [Required, MaxLength(50), RegularExpression("^[^<>]+$")]
    public string Name { get; set; }

    [Required]
    public int SubScore { get; set; }

    [MaxLength(500), RegularExpression("^[^<>]+$")]
    public string OpenQuestions { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    [MaxLength(50), RegularExpression("^[^<>]+$")]
    public string UserId { get; set; }

    public List<PoolMessage> PoolMessages { get; set; }
    public List<MatchPrediction> MatchPredictions { get; set; }
    public List<FinalsPrediction> FinalsPredictions { get; set; }
  }


    /* Defaults property cannot via Annotations, only through FluentApi
    public class CoreContext : DbContext
    {
        public DbSet<PoolPlayer> PoolPlayer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PoolPlayer>()
                .Property(b => b.Subscore)
                .HasDefaultValue(0);
        }
    }
     */
}

/*
CREATE TABLE [dbo].[PoolPlayer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SubScore] [int] NOT NULL,
	[OpenQuestions] [nvarchar](500) NULL,
	[UserID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PoolPlayer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 */

/*
ALTER TABLE [dbo].[PoolPlayer] ADD  CONSTRAINT [DF_PoolPlayer_Score]  DEFAULT ((0)) FOR [SubScore]
ALTER TABLE [dbo].[PoolPlayer]  WITH CHECK ADD  CONSTRAINT [FK_PoolPlayer_aspnet_Users] FOREIGN KEY([UserID]) REFERENCES [dbo].[aspnet_Users] ([UserId])
  */
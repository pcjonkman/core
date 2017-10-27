using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Pool
{
  public class MatchPrediction : Entity {
    // [Required, ForeignKey("MatchId")]
    // public virtual Match Match { get; set; }

    [Required]
    public int MatchId { get; set; }

    // [Required, ForeignKey("PoolPlayerId")]
    // public virtual PoolPlayer PoolPlayer { get; set; }

    [Required]
    public int PoolPlayerId { get; set; }

    public int SubScore { get; set; }

    [Range(0, 20)]
    public int GoalsCountry1 { get; set; }

    [Range(0, 20)]
    public int GoalsCountry2 { get; set; }
  }

    /* Defaults property cannot via Annotations, only through FluentApi
    public class CoreContext : DbContext
    {
        public DbSet<MatchPrediction> MatchPrediction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MatchPrediction>()
                .Property(b => b.Subscore)
                .HasDefaultValue(0);
        }
    }
     */
}

/*
CREATE TABLE [dbo].[MatchPrediction](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Match] [int] NOT NULL,
	[Subscore] [int] NULL,
	[PoolPlayer] [int] NOT NULL,
	[GoalsCountry1] [int] NULL,
	[GoalsCountry2] [int] NULL,
 CONSTRAINT [PK_MatchPrediction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 */

/*
ALTER TABLE [dbo].[MatchPrediction]  WITH CHECK ADD  CONSTRAINT [FK_MatchPrediction_Match] FOREIGN KEY([Match]) REFERENCES [dbo].[Match] ([ID])
ALTER TABLE [dbo].[MatchPrediction]  WITH CHECK ADD  CONSTRAINT [FK_MatchPrediction_MatchPrediction] FOREIGN KEY([PoolPlayer]) REFERENCES [dbo].[PoolPlayer] ([ID])
  */
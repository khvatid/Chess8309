namespace ChessServer.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelChessDB : DbContext
    {
        public ModelChessDB()
            : base("name=ModelChessDB")
        {
        }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<PlayerProfile> PlayerProfiles { get; set; }
        public virtual DbSet<Side> Sides { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<BlackPlayer> BlackPlayers { get; set; }
        public virtual DbSet<TrueGamesInSystem> TrueGamesInSystems { get; set; }
        public virtual DbSet<WhitePlayer> WhitePlayers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .Property(e => e.Fen)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .HasMany(e => e.Sides)
                .WithRequired(e => e.Game)
                .HasForeignKey(e => e.Game_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlayerProfile>()
                .Property(e => e.emailPlayer)
                .IsUnicode(false);

            modelBuilder.Entity<PlayerProfile>()
                .Property(e => e.namePlayer)
                .IsUnicode(false);

            modelBuilder.Entity<PlayerProfile>()
                .Property(e => e.passwordPlayer)
                .IsUnicode(false);

            modelBuilder.Entity<PlayerProfile>()
                .HasMany(e => e.Sides)
                .WithRequired(e => e.PlayerProfile)
                .HasForeignKey(e => e.Player_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Side>()
                .Property(e => e.Player_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Side>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<BlackPlayer>()
                .Property(e => e.blackPlayer1)
                .IsUnicode(false);

            modelBuilder.Entity<BlackPlayer>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<TrueGamesInSystem>()
                .Property(e => e.Player_ID)
                .IsUnicode(false);

            modelBuilder.Entity<TrueGamesInSystem>()
                .Property(e => e.namePlayer)
                .IsUnicode(false);

            modelBuilder.Entity<WhitePlayer>()
                .Property(e => e.whitePlayer1)
                .IsUnicode(false);

            modelBuilder.Entity<WhitePlayer>()
                .Property(e => e.Color)
                .IsUnicode(false);
        }
    }
}

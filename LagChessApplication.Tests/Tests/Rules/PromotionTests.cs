﻿using LagChessApplication.Domains;
using LagChessApplication.Domains.Enums;
using LagChessApplication.Extensions;
using Xunit;

namespace LagChessApplication.Tests.Tests.Rules
{
    public class PromotionTests
    {
        [Fact]
        public void Pawn_ShouldBePromoted_WhenArrivingAtPromotionRow()
        {
            var chessGame = GameChessExtension.Create();

            chessGame.Board.OnPawnPromotion += () => PieceTypeEnum.Queen;

            chessGame.Play(Square.B2, Square.B3);
            chessGame.Play(Square.A7, Square.A6);

            chessGame.Play(Square.B3, Square.B4);
            chessGame.Play(Square.A6, Square.A5);

            chessGame.Play(Square.B4, Square.B5);
            chessGame.Play(Square.A5, Square.A4);

            chessGame.Play(Square.B5, Square.B6);
            chessGame.Play(Square.A4, Square.A3);

            chessGame.Play(Square.B6, Square.C7);
            chessGame.Play(Square.A8, Square.A4);

            chessGame.Play(Square.C7, Square.B8);
            chessGame.Play(Square.A4, Square.H4);

            chessGame.Play(Square.B8, Square.C8);
            chessGame.Play(Square.H4, Square.H2);
        }
    }
}

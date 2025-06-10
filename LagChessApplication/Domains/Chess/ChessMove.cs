using LagChessApplication.Domains.Enums;
using System.Text;

namespace LagChessApplication.Domains.Chess
{
    public readonly struct ChessMove(Square from, Square to, PieceTypeEnum piece, bool opponentKingInCheck, bool opponentKingInCheckMate, bool capturedPiece, PieceTypeEnum? pawnPromotion)
    {
        public readonly Square From { get; } = from;
        public readonly Square To { get; } = to;
        public readonly PieceTypeEnum Piece { get; } = piece;

        public readonly bool OpponentKingInCheck { get; } = opponentKingInCheck;
        public readonly bool OpponentKingInCheckMate { get; } = opponentKingInCheckMate;
        public readonly bool CapturedPiece { get; } = capturedPiece;
        public readonly PieceTypeEnum? PawnPromotion { get; } = pawnPromotion;

        public string Notation
        {
            get
            {
                var notationBuilder = new StringBuilder();

                var key = GetPieceKey(Piece);

                if (key.HasValue)
                {
                    notationBuilder.Append(key);
                }

                if (CapturedPiece)
                {
                    if (Piece == PieceTypeEnum.Pawn)
                    {
                        notationBuilder.Append(From.Column);
                    }

                    notationBuilder.Append('x');
                }

                notationBuilder.Append(To.ToString());

                if (PawnPromotion.HasValue)
                {
                    var promotionKey = GetPieceKey(PawnPromotion.Value);

                    notationBuilder.Append('=');
                    notationBuilder.Append(promotionKey);
                }

                if (OpponentKingInCheckMate)
                {
                    notationBuilder.Append('#');
                }
                else if (OpponentKingInCheck)
                {
                    notationBuilder.Append('+');
                }

                return notationBuilder.ToString();
            }
        }

        private static char? GetPieceKey(PieceTypeEnum pieceType)
        {
            return pieceType switch
            {
                PieceTypeEnum.Pawn => null,
                PieceTypeEnum.Knight => 'N',
                PieceTypeEnum.Bishop => 'B',
                PieceTypeEnum.Rook => 'R',
                PieceTypeEnum.Queen => 'Q',
                PieceTypeEnum.King => 'K',

                _ => throw new NotImplementedException()
            };
        }

        public static ChessMove Create(Square from, Square to, PieceTypeEnum piece, bool opponentKingInCheck, bool opponentKingInCheckMate, bool capturedPiece, PieceTypeEnum? pawnPromotion) => 
            new(from, to, piece, opponentKingInCheck, opponentKingInCheckMate, capturedPiece, pawnPromotion);
    }
}

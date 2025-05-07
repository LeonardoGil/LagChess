using LagChessApplication.Domains.Enums;

namespace LagChessApplication.Domains.Chess
{
    public readonly struct ChessMove(Square from, Square to, PieceTypeEnum piece, bool capturedPiece, PieceTypeEnum? pawnPromotion)
    {
        public readonly Square From { get; } = from;
        public readonly Square To { get; } = to;
        public readonly PieceTypeEnum Piece { get; } = piece;

        public readonly bool CapturedPiece { get; } = capturedPiece;
        public readonly PieceTypeEnum? PawnPromotion { get; } = pawnPromotion;

        public string Notation 
        { 
            get
            {
                return string.Empty;
            }
        }

        public static ChessMove Create(Square from, Square to, PieceTypeEnum piece, bool capturedPiece, PieceTypeEnum? pawnPromotion) => new(from, to, piece, capturedPiece, pawnPromotion);
    }
}

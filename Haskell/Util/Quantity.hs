module Util.Quantity (
    Quantity,
    quantity,
    fromQuantity
) where
    
    newtype Quantity = Quantity Integer

    instance Show Quantity where
        show (Quantity n) = show n

    instance Ord Quantity where
        Quantity x < Quantity y = x < y
        Quantity x <= Quantity y = x <= y
        Quantity x > Quantity y = x > y
        Quantity x >= Quantity y = x >= y

    instance Eq Quantity where
        Quantity x == Quantity y = x == y

    instance Num Quantity where
        Quantity x + Quantity y = Quantity $ x + y
        Quantity x * Quantity y = Quantity $ x * y
        Quantity x - Quantity y
            | c < 0     = error "We don't support negative quantities."
            | otherwise = Quantity c
            where
                c = x - y

        negate q@(Quantity x)
            | x == 0    = q
            | otherwise = error "We don't support negative quantities."

        abs = id

        signum (Quantity x)
            | x > 0     = Quantity 1
            | x == 0    = Quantity 0
            | otherwise = error "We don't support negative quantities."

        fromInteger n = if n >= 0 then Quantity n else error "We don't support negative quantities."

        
    quantity :: Integer -> Maybe Quantity
    quantity n = if n >= 0 then Just $ Quantity n else Nothing

    fromQuantity :: Quantity -> Integer
    fromQuantity (Quantity n) = n
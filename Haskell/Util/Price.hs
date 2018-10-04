module Util.Price (
    Price,
    price,
    fromPrice
) where
    import Data.Ratio

    newtype Price = Price Rational

    instance Show Price where
        show (Price n) = show $ fromRational $ n

    instance Num Price where
        Price a + Price b = Price $ a + b
        Price a * Price b = Price $ a * b
        Price a - Price b
            | c < 0     = error "We don't support negative prices."
            | otherwise = Price c
            where
                c = a - b
        negate p@(Price a)
            | a == 0    = p
            | otherwise = error "We don't support negative prices."
        abs = id
        signum (Price a)
            | a > 0  = Price $ toRational 1
            | a == 0 = Price $ toRational 0
            | otherwise = error "We don't support negative prices."

        fromInteger n = if n >= 0 then Price $ toRational n else error "We don't support negative prices."

    price :: Real a => a -> Maybe Price
    price amount = if amount >= 0 then Just $ Price $ toRational amount else Nothing

    fromPrice :: Price -> Rational
    fromPrice (Price x) = x
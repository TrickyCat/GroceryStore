module PosTerminal (
    scanCode,
    revokeCode,
    calcTotal
) where
    import qualified Calculator
    import Cart (CartItem (CartItem), Cart, addItemToCart, removeItemFromCart)
    import Util.Price (Price)
    import Control.Monad.State

    addItemToCart' :: CartItem -> Cart -> Cart
    addItemToCart' = flip addItemToCart

    removeItemFromCart' :: CartItem -> Cart -> Cart
    removeItemFromCart' = flip removeItemFromCart

    scanCode :: String -> State Cart ()
    scanCode code = modify (addItemToCart' $ CartItem code)

    revokeCode :: String -> State Cart ()
    revokeCode code = modify (removeItemFromCart' $ CartItem code)
        
    calcTotal :: State Cart Price
    calcTotal = do
        cart <- get
        return $ Calculator.calcTotal cart
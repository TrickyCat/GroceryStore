module Cart (
    CartItem (CartItem),
    Cart,
    cartItemCode,
    getCartItems,
    emptyCart,
    addItemToCart,
    removeItemFromCart
) where

import Data.List (delete)

newtype CartItem = CartItem { cartItemCode :: String }
    deriving (Show, Eq)

newtype Cart = Cart { getCartItems :: [CartItem] }
    deriving (Show)

emptyCart :: Cart
emptyCart = Cart []

addItemToCart :: Cart -> CartItem -> Cart
addItemToCart (Cart items) item = Cart $ item : items

removeItemFromCart :: Cart -> CartItem -> Cart
removeItemFromCart (Cart items) item = Cart $ delete item items
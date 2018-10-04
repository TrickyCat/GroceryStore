module Calculator (
    calcTotal
) where
    import Cart (Cart, getCartItems, cartItemCode)
    import Util.Price (Price)
    import Products.Product (Product, ProductCode, unitPrice)
    import Products.ProductRepository (getProductByCode)
    import Products.ProductOffer (ProductOffer, OfferMatchResult(OfferMatchResult), matchOffer, success, remainingProductQty, 
            matchedOfferCode, code, offerPriceTimesQty)
    import Products.ProductOfferRepository (getOffersByProductCode)
    import Data.List (filter, map)
    import qualified Data.Map as M
    import Data.Maybe (isJust, isNothing)
    import Util.Misc (lowercase)
    import Util.Quantity (Quantity, fromQuantity)

    calcTotal :: Cart -> Price
    calcTotal cart = let
        cartItems = getCartItems cart
        groupedItemCodes = groupCodes $ map cartItemCode cartItems
        total = M.foldrWithKey' (\productCode productQty acc ->
            acc + handleProduct productCode productQty
            ) 
            (fromInteger 0 :: Price)
            groupedItemCodes
        in
        total
       
    one :: Quantity
    one = fromInteger 1

    groupCodes :: [ProductCode] -> M.Map ProductCode Quantity
    groupCodes productCodes =
        foldr (\c m -> let
            updater Nothing      = Just one
            updater (Just count) = Just $ count + one
            in
            M.alter updater (lowercase c) m)
        M.empty
        productCodes


    handleProduct :: ProductCode -> Quantity -> Price
    handleProduct productCode productQty = let
        offers = getOffersByProductCode productCode
        in
        if null offers then
            handleProductWithoutOffers productCode productQty
        else
            handleProductWithOffers productCode productQty offers


    handleProductWithOffers :: Foldable t => ProductCode -> Quantity -> t ProductOffer -> Price
    handleProductWithOffers productCode productQty offers = let
        (productQty', appliedOffers) = foldr (\offer acc@(qty, m) -> let

            offerMatcher macc@(remainingQty, matchedQty) = case matchOffer offer productCode remainingQty of
                    OfferMatchResult{ success = s, remainingProductQty = rpq } | s -> offerMatcher (rpq, matchedQty + 1)
                    _                                                              -> macc
            
            (remProductQty, appliedOffersQty) = offerMatcher (qty, 0)
            in
                if appliedOffersQty == 0 then
                    acc
                else
                    (remProductQty, M.insert (code offer) (offer, appliedOffersQty) m)

            ) (productQty, M.empty) offers

        totalPriceFromAppliedOffers = M.foldr' (\(offer, qty) s -> s + offerPriceTimesQty offer qty) (fromInteger 0) appliedOffers
        totalPriceForRemainingProducts = handleProductWithoutOffers productCode productQty'
        in
            totalPriceFromAppliedOffers + totalPriceForRemainingProducts


    handleProductWithoutOffers :: ProductCode -> Quantity -> Price
    handleProductWithoutOffers productCode productQty = let
        maybeProduct = getProductByCode productCode
        (Just product) = maybeProduct
        in
        if isNothing maybeProduct then
            error $ "Product with code " ++ productCode ++ " not found."
        else
            unitPrice product * (fromInteger . fromQuantity $ productQty)


    
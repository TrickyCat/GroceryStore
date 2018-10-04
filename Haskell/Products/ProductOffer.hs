module Products.ProductOffer (
    ProductOffer(ProductOffer),
    OfferMatchResult(OfferMatchResult),
    productCode,
    matchOffer,
    success,
    remainingProductQty,
    matchedOfferCode,
    code,
    offerPriceTimesQty
    
) where

    import Util.Price (Price)
    import Util.Quantity (Quantity, fromQuantity)
    import Util.Misc (lowercase)
    import Products.Product (ProductCode)

    type OfferCode = String
    type OfferTitle = String

    data ProductOffer = ProductOffer {
        code        :: OfferCode,
        title       :: OfferTitle,
        offerPrice  :: Price,
        productCode :: ProductCode,
        productQty  :: Quantity
    }
        deriving Show

    data OfferMatchResult = OfferMatchResult {
        success             :: Bool,
        matchedOfferPrice   :: Price,
        matchedOfferCode    :: OfferCode,
        remainingProductQty :: Quantity
    }
        deriving Show

    offerPriceTimesQty :: ProductOffer -> Quantity -> Price
    offerPriceTimesQty (ProductOffer { offerPrice = oPrice }) qty = let
        qtyN = fromQuantity qty
        in
            oPrice * (fromInteger qtyN)

    matchOffer :: ProductOffer -> ProductCode -> Quantity -> OfferMatchResult
    matchOffer (ProductOffer { productCode = pc, productQty = pq, offerPrice = op, code = oc }) productCode productQty =
        if lowercase productCode == lowercase pc && productQty >= pq then
            OfferMatchResult { success = True, matchedOfferPrice = op, matchedOfferCode = oc, remainingProductQty = productQty - pq }
        else
            OfferMatchResult { success = False, matchedOfferPrice = op, matchedOfferCode = oc, remainingProductQty = productQty }

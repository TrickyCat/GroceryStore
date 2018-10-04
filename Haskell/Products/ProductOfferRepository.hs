module Products.ProductOfferRepository (
    getOffersByProductCode
) where
    import Products.ProductOffer (ProductOffer(ProductOffer), productCode)
    import Data.Map (Map, empty, alter, findWithDefault)
    import Util.Price (price)
    import Data.List (map, filter)
    import Data.Maybe (Maybe(Just), isJust)
    import Data.Function ((&))
    import Util.Quantity (quantity)
    import Products.Product (ProductCode)
    import Util.Misc (lowercase)

    type ProductQty = Integer
    type FakeOfferTitle = String
    type FakeOfferCode = String
    type FakeRepoOffer a = (FakeOfferCode, FakeOfferTitle, a, ProductCode, ProductQty) 

    rawOffers :: [FakeRepoOffer Double]
    rawOffers = [
        ("OFFER_AVOCADO_FALL_2018", "Incredible chance to buy avocados.", 3, "A", 3),
        ("OFFER_CARROT_FALL_2018", "Buy more carrots for less money.", 5, "C", 6)
        ]

    offers :: Map ProductCode [ProductOffer]
    offers = 
        rawOffers
        & map (\(code, title, offerPrice, productCode, productQty) -> do
            offerPrice' <- price offerPrice
            productQty' <- quantity productQty
            return $ ProductOffer code title offerPrice' (lowercase productCode) productQty')
        & filter isJust
        & foldr (\(Just offer@(ProductOffer { productCode = pc })) map -> 
            let
                updater Nothing       = Just [offer]
                updater (Just offers) = Just $ offer : offers
            in
            alter updater pc map
            ) empty
        

    getOffersByProductCode :: ProductCode -> [ProductOffer]
    getOffersByProductCode code = findWithDefault [] (lowercase code) offers
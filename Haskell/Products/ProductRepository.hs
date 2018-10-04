module Products.ProductRepository (
    getProductByCode
) where
    import Prelude hiding (lookup)
    import Products.Product (Product(ProductR), title, code, unitPrice)
    import Data.Map (Map, fromList, lookup)
    import Util.Price (price)
    import Data.List (map, filter)
    import Data.Maybe (Maybe(Just), isJust)
    import Data.Function ((&))
    import Products.Product (ProductCode)
    import Util.Misc (lowercase)

    type ProductTitle = String
    type RawProductPrice = Double
    type FakeRepoProduct = (ProductCode, ProductTitle, RawProductPrice)

    rawProducts :: [FakeRepoProduct]
    rawProducts = [
        ("A",   "Avocado",    1.25),
        ("B",   "Beetroot",   4.25),
        ("C",   "Carrot",     1),
        ("D",   "Durian",     0.75),

        ("M",   "Melon",      2.15),
        ("W",   "Watermelon", 1.65),
        ("MNG", "Mango",      2.95),
        ("G",   "Guava",      1.95),
        ("P",   "Pineapple",  3.95)
        ]
    
    products :: Map ProductCode Product
    products = 
        rawProducts
        & map (\(code, title, itemPrice) -> do {p <- price itemPrice; return ProductR { code = lowercase code, title = title, unitPrice = p }})
        & filter isJust
        & map (\(Just product) -> (code product, product))
        & fromList

    getProductByCode :: ProductCode -> Maybe Product
    getProductByCode code = lookup (lowercase code) products

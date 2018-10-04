module Products.Product (
    Product(ProductR),
    title, code, unitPrice,
    ProductCode
) where

    import Util.Price (Price)
    
    type ProductCode = String
    type ProductTitle = String

    data Product = ProductR {
        code      :: ProductCode,
        title     :: ProductTitle,
        unitPrice :: Price
    }
        deriving Show


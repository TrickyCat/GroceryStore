module Util.Misc (
    lowercase
) where
    import Data.Char (toLower)
    import Data.List (map)

    lowercase :: String -> String
    lowercase = map toLower
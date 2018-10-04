module Main where

import Tests

main :: IO ()    
main = do
    greet
    runTestsAndShowResult    
    bye

greet :: IO ()
greet = putStrLn "\nHello, YouScan!\n"

bye :: IO ()
bye = putStrLn "\nBye, YouScan!\n"

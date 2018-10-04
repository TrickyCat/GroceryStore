module Tests (
    runTestsAndShowResult
) where

    import Cart (Cart, emptyCart)
    import PosTerminal (scanCode, revokeCode, calcTotal)
    import Util.Price (Price, fromPrice)
    import Control.Monad.State
    
    type ExpectedTotal = Double
    type TestScanInput = [String]
    type RevokedScanInput = [String]
    type TestDescription = String
    data TestEntry = TestEntry { 
        testDescription  :: TestDescription,
        testScanInput    :: TestScanInput,
        revokedScanInput :: RevokedScanInput,
        expectedTotal    :: ExpectedTotal
        }
        deriving Show
    type TestData = [TestEntry]
    
    testCases :: TestData
    testCases = [
        TestEntry "Scan these items in this order: ABCDABA. Verify the total price is 13.25" (map (:[]) "ABCDABA") [] 13.25,
        TestEntry "Scan these items in this order: CCCCCCC. Verify the total price is 6.00" (map (:[]) "CCCCCCC") [] 6,
        TestEntry "Scan these items in this order: ABCD. Verify the total price is 7.25" (map (:[]) "ABCD") [] 7.25,
    
        TestEntry "Scan these items in this order: ABCDABAABCDABA. Then revoke these items: ABCDABA. Verify the total price is 13.25." 
            (map (:[]) "ABCDABAABCDABA") (map (:[]) "ABCDABA") 13.25,
        TestEntry "Scan these items in this order: CCCCCCCCCCCCCC. Then revoke these items: CCCCCCC. Verify the total price is 6.00." 
            (map (:[]) "CCCCCCCCCCCCCC") (map (:[]) "CCCCCCC") 6,
        TestEntry "Scan these items in this order: ABCDABCD. Then revoke these items: ABCD. Verify the total price is 7.25" 
            (map (:[]) "ABCDABCD") (map (:[]) "ABCD") 7.25,

        TestEntry "Scan these items in this order: AAAAAA. Verify the total price is 6" (map (:[]) "AAAAAA") [] 6
        ]

    runTestsAndShowResult :: IO ()
    runTestsAndShowResult = do
        allPass <- runTests
        putStrLn $ "\nAll tests pass: " ++ show allPass
    
    runTests :: IO Bool
    runTests = do
        results <- mapM runTest testCases
        return $ all id results
    
    
    runTest :: TestEntry -> IO Bool
    runTest testEntry = do
        putStrLn $ "========================================="
        putStrLn $ "Test: " ++ testDescription testEntry ++ "\n"
    
        let expected = toRational $ expectedTotal testEntry
        let actual = evalState (getTotal (testScanInput testEntry) (revokedScanInput testEntry)) emptyCart
        let pass = fromPrice actual == expected
    
        putStrLn $ "Expected: " ++ (show . fromRational) expected
        putStrLn $ "Actual:   " ++ show actual
        putStrLn $ "Pass:     " ++ show pass
    
        putStrLn $ "========================================="
        return pass
    
    
    getTotal :: TestScanInput -> RevokedScanInput -> State Cart Price
    getTotal scanInput revokedInput = do
        mapM_ scanCode scanInput
        mapM_ revokeCode revokedInput
        calcTotal
        
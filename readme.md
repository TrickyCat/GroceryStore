## Task Description

> Consider a grocery market where items have prices per unit but also volume prices. For example, doughnuts may be $1.25 each or 3 for $3 dollars. There could only be a single volume discount per product.
>
> Implement a point-of-sale scanning API (library) that accepts an arbitrary ordering of products (similar to what would happen when actually at a checkout line) then returns the correct total price for an entire shopping cart based on the per unit prices or the volume prices as applicable.
>
> Here are the products listed by code and the prices to use (there is no sales tax):
> 
> Product Code | Price	
> ------------ | ---------
> A	         | $1.25 each or 3 for $3.00	
> B	         | $4.25	
> C	         | $1.00 or $5 for a six pack	
> D	         | $0.75	
> 
> The library at the top level PointOfSaleTerminal service object should look something like this. You are free to design/implement the rest of the code however you wish, including how you specify the prices in the system:
> 
> ```csharp
> PointOfSaleTerminal terminal = new PointOfSaleTerminal();
> terminal.SetPricing(...);
> terminal.Scan("A");
> terminal.Scan("C");
> //... etc.
> double result = terminal.CalculateTotal();
> ```
> Here are the minimal inputs you should use for your test cases. These test cases must be shown to work in your program:
> ```
> Scan these items in this order: ABCDABA; Verify the total price is $13.25.
> Scan these items in this order: CCCCCCC; Verify the total price is $6.00.
> Scan these items in this order: ABCD; Verify the total price is $7.25
> ```
> 

## Remarks
Формулировка задания допускает вольность трактовки:
* *__There could only be a single volume discount per product.__* Где конкретно "может быть одна скидка (один product offer)" неясно: 
    * не то в хранилище данных в любой конкретный момент времени
    * не то это инвариант бизнес процесса продажи
    * не то в коде решения даже при наличии "конкурирующих" офферов для одного продукта стоит всегда брать один из них, тогда критерий его выбора также может быть разным
    * не то скидка (оффер) может быть применена к товарам в корзине лишь единожды даже при потенциальной возможности множественного применения, если нужных товаров в корзине достаточное количество
    * etc

Посему командой вымышленной компании, разрабатывающей ПО для нашей точки продажи овощей и фруктов, ввиду неопреденности требований, было принято решение иметь возможность указывать несколько офферов (one product to many offers) и офферы могут быть применены несколько раз к корзине покупателя. Стратегия выбора порядка применения офферов для продукта к корзине в первом спринте нашей гипотетической командой была выбрана наипростейшей: очередность применяемых офферов задается порядком данных, возвращаемых из репозитория, что может не быть оптимальной стратегией применения офферов и вполне может быть изменено в следующем спринте после фидбеков от тестировщиков\продакт оунеров\кастомеров\аналитиков.

Команда пошла по пути создания наименее трудозатратных и наиболее простых, чуть ли не тривиальных, вариантов реализации и прочих компонентов системы, ведь спринт-то первый и обратной связи еще нет, такое себе подобие MVP гордо запилила команда за первую итерацию.

Более того, по факту было создано аж две MVP-шки, кому какая больше по душе (вот такая у нас проактивная команда в сферической компании!). Средства реализации одного из решений обычно окутаны дымкой таинственности, чуть ли не мистичности, и по миру ходят слухи, что там нужно нехило так знать теорию графов, категорий, лямбда исчисление, сопромат, анатомию головного мозга, теоретическую физику и прочий матан даже для вывода строки на консоль и написать что-то "по-настоящему работающее" там нельзя. Но наша команда не пошла на поводу у толпы и такую чушь в голову не брала, за что им честь и хвала, а также бонус от компании.

Более того, команда пошла по брутальному пути разработки и не стала прибегать к использованию сторонних библиотек и пакетов от слова "совсем".

---

На последующих спринтах возможно много изменений:
* для простоты первого решения многие guard checks (aka defensive coding) были опущены в угоду читаемости, но перед гипотетическим publish to production environment были бы добавлены
* уточнение приоритета разных офферов для одного продукта, стратегия их выбора
* обобщение офферов "single product type offer" -> "multiple product types offer":
    ```
    Offer { 3 avocados for 3$ }
    
     |
     |
     |
    \_/
    
    Offer { 2 avocados + 10 apples + 1 pear for 3.25$ } 
    ```
* для офферов можно дополнительно указывать период (или периоды) активности, в простейшем варианте:
    ```
    Offer {
        <FIELDS>
        StartDateTimeOffset,
        EndDateTimeOffset
    }
    ```

* добавление валют, если в этом есть небходимость, которые были опущены в первой реализации
* если, к примеру, офферы хранятся в RDBMS, то по истечении оффера (по прошествии всех периодов его активности) можно добавить ему отметку об архивности либо перенести в другую архивную таблицу либо в другое хранилище для отчетности\контроля (e.g. OLTP -> OLAP, DWH)
* and much more

---
### Запуск решений
1. При наличии Visual Studio двойной щелчок по __.sln__ файлу должен сделать свое дело и потом __F5/Ctrl+F5__ либо собрать из-под консоли тем же __MSBuild__'ом и запустить

2. Варианта два:
    * скомпилировать и запустить
    ```
    λ cd <IMPLEMENTATION_FOLDER>
    λ ghc main
    λ main
    ```
    
    * проинтерпретировать и запустить
    ```
    λ cd <IMPLEMENTATION_FOLDER>
    λ ghci main
    *Main> main
    ```
    
    * третий бонусный вариант: не запускать, а просто смотреть на код и постигать дзен )))
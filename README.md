So before we begin diving into the read me file there's a few key notes I'd like to discuss.

I was deciding between Google API implementation vs web scraping however, web scraping is against Googles terms and conditions, so I've opted for the correct fully functioning implementation using Google's "Custom Search JSON API" and their "Programmable Search Engine".

Please keep in mind, that Google API custom search can "nearly" meet the implementation that google uses, however it will not be exactly the same.

Long term scraping of a website, can lead to eventual IP Bans and legal action.

Traditionally, I would have these keys in some kind of Azure KeyVault or AWS equivalent instead of stored just in a config file.

Enjoy the read :) 

**IMPORTANT NOTE:** The key provided will cut off after 100 search queries per day. (So careful with usage, or just make more keys)

# SEO Checker

SEO Checker is a console application built in C# that uses the Google Custom Search JSON API to check the search ranking of a specific URL for a given set of keywords. This tool automates the process of checking how well a website ranks on Google for specific search terms, displaying the ranking positions if the URL appears within the first 100 results.

## Features

- **Google Custom Search Integration**: Uses Google’s Custom Search JSON API to retrieve search results.
- **Caching**: Stores results in memory to avoid redundant API calls for the same keywords and URL.
- **Asynchronous Operations**: Makes non-blocking API requests for efficient performance.
- **Modular Design**: Organized with interfaces and services, making it extensible and maintainable.

## Prerequisites

- **Google API Key** and **Custom Search Engine ID (CSE ID)** for the Google Custom Search API

## Unit Tests
- ![image](https://github.com/user-attachments/assets/bd99dd45-bd58-46a9-ae4e-108acb8c3483)

## Search Results (Differentiation between showing 2 and 1 can be explained by the comments above!)
![image](https://github.com/user-attachments/assets/5c526faa-0ef5-49ad-b817-45d4a7aff225)

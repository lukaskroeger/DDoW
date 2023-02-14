# Daily Dose of Wikipedia
Your **Daily Dose of Wikipedia** is a .NET MAUI App that provides you with Wikipedia articles you may find interesting.
The app initially loads five random Wikipedia articles queried from the [Wikipedia Random API](https://www.mediawiki.org/wiki/API:Random#).
The articles are shown in a card stack, where each card contains an excerpt of an Article. You can read the excerpt and decide if if you:
- want to **read more** \
  A web view will open that show the complete article.
- **like** the article (swipe right)\
  A service is queried with the article you liked. The service will return articles that are connected to the liked article. The returned articles are added to
  the end of the card stack.
- **dislike** the article (swipe left) \
  The article is simply removed from the stack. Nothing more happens at the moment. In the future it could make sense to use dislikes to improve the recomendations

The App uses the Wikipedia Recommendation Service. Currently thats a very simple service that simply returns articles that are linked in the "See also" section 
of an article. I have planned to add more ways to find recommentations in the future.

The app has currently no database and thus doesn't persist any recommended articles between app starts. The recommended articles are only persisted during the execution of the app.
This is another point I want to address in the future.

The whole project is only a fun project I do in my free time to learn something. If you want to contribute or have any ideas or bugs, feel free to open issues.

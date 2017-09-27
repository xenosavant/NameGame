# The Namegame: C# / Xamarin

This is the standard NameGame with a few modifications. I implemented the app in Xamarin using MvvmCross on the Android Platform.

## Features

The app allows the user to get hints by pushing the "Hint" button. This removes one of the wrong choices until only two remain.

The app tracks the user's score as the percentage of correct answers out of their total guesses.

The app has two game modes: standard and timed.

1. Standard mode allows the user to guess the person until they get it right and then move on to the next set.

2. Timed mode gives the user a short period of time to guess the correct person. It implements an adaptive algorithm that increases the time limit based on the user's score. As the user gets more guesses right, the time window decreases...guess quickly!!  
# MusicLovers
> this appllcation allows users to create reviews for albums and songs, as well as see what other people had to say about them

## How to Use
> Album/Create
> 1. Check if the album artist has already been created
> 2. if not create one (Artict/Create)
> 3. Create an album by inputing its title, selecting the artist, and inputting the genre and release date
> 4. Click done, and you'll move to Create/Track
> 5. Enter track titles in order that they appear on the album, then click "Add Track to Album"
> 6. Repeat until all tracks have been created
> 7. Click "Done", and you will get an Album/Detail page with all the infi you just input

>Review/Create
>
>Functions the same for Album and Track reviews
>Must be logged in
>1. Select Album or Track from list (will populate Id value in request)
>2. Enter a rating and comment, then hit submit
>3. Index (NewIndex) page will update with new score added to the album or track's average score

### Albums
- when creating, add tracks in the order that they appear on the album
- users cannot delete or modify albums or tracks, as other users may have reviews for them

### Tracks
- all info other than title populates from its FK relationship with *Album*
- no option to just create a track, all tracks must be associated with an album
- if a track is a single, create an album with *-Single* after the title

### Reviews
- Album and Track reviews function the exact same way, and both inherit from Base Review class. The only difference is what they have a FK relationship with
- Click **Reviews** on NewIndex page for reviews to see all reviews associated with that specific album or track
- *Edit* & *Delete* options will not be displayed if you are not logged in or do not own the review

## Search Bar on Tables
> filters through the selected column and hides all objects that do not match the search query

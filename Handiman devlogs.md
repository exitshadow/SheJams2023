# Handiman devlogs

## Devlog #1 — Creating dialogue boxes that automatically position in screen space

Making dialogue boxes in Unity can be quite frustrating. Since *Handiman* won the SheJams International Prize, we wanted to polish and expand the game to be a real little pearl. One of the feedbacks that kept coming up most frequently from play testers was that the dialogue boxes were confusing and rigid, which was true—there basically wasn’t anything else but direct text injection to signal the player someone else is talking. Given that in *Handiman*, dialogues are sort of the poster star, that had to be taken care of.

Here’s how.

### Animated text

First, animated text is something that can go a long way to make text feel nicer and snappier. We decided to go with TextAnimator, a well-known asset for games in Unity developed by ((Fabiolucci)) (verify name). It allows to easily implement text effects with a lot of granularity and not too much migraine. To be honest, it is practically automatic! Totally worth its price.

(detail detail later)

### Dialogue boxes dynamically scaled and placed on screen

Now comes the second—and biggest—challenge. Most dialogue systems would simply be an overlay in some fixed part of the screen, generally in the bottom centre. This is what the game was like when submitted to the jam, and it was confusing because the text would just appear in there without any other effect. It is confusing because it is uncertain who is talking since the colours, location or images don’t change depending on the character that is speaking.

One solution would to create two different dialogue boxes, one for the main protagonist Iman, and one for any other character speaking. Another one would be to colour-code each box according to the character speaking, and even characters avatars—a solution that has been seen in Donut County, by example.

However, in Handiman we have quite a lot of stuff happening right on that coveted bottom-centre space of the screen. So not only our current system is unclear, it also covers up a huge part of the game’s charming décor. Also, due to the future implementation of short cinematics, it is quite hard to really anticipate where we will effectively need the boxes unless we manually place and animate them every time, which will be tricky to adjust and time-consuming.

So why not going full-on with a UI system that not only scales the box according to the text’s length, but also is capable of dynamically place them on the screen space without ever covering the characters on screen? Of course yes. Of course, let’s go for the most complicated system. Everything will be alright.

#### Automatic scaling

This part has proven to be easier. Assigning a vertical layout group that controls the text width and automatically rescaling on overflow worked out well. It doesn’t allow for rescaling on the horizontal width, but it proves sufficient and reliable for all types of dialogue lines, and easy to implement. Solution adopted.

#### Automatic placing

##### Super basic draft

This part is clearly the most complex one. At first, I implemented a system that simply uses anchors on the player or objects in world-space, that are then converted in screen space and used as the origin point of the newly formed dialogue box.

In order to mimic the back-and-forth sensation that a dialogue can have and also sort of manually keep the dialogue within the screen, the pivots of the anchor change.

It is an extremely simple system that is very reliable, feels surprisingly fluid and doesn’t jump unless the camera does. It still isn’t perfect, but it is a massive improvement since the last version.

```csharp
    public void PlaceDialogueBoxInScreen()
    {
        // world position to screen
        Vector2 dialogueScreenPosition = Camera.main.WorldToScreenPoint(currentDialogueAnchor.position);
        
        if (currentDialogueAnchor == playerDialogueAnchor)
        {
            dialogueBoxGroup.pivot = new Vector2(0, 0); // pivot bottom left 
        }
        else dialogueBoxGroup.pivot = new Vector2(1, 0); // pivot bottom right
        
        dialogueBoxGroup.anchoredPosition = dialogueScreenPosition;

    }
```

Now, it does present a pretty significant problem: depending on where the player is looking, the boxes can go off-screen or cover the characters that are supposedly speaking, which is quite ugly and confusing. In this system, the designer has to set the references to `playerDialogueAnchor`and `dialogueAnchor` manually and adjust values for it to look okay.

The challenging task now is to devise a way for the boxes to limit themselves within a margin in the screen and around the characters, so we can pass any anchor coordinates to the UI and the boxes will always be nicely placed.

##### Delimiting in screen space

First we have to delimit the screen space. We want the dialogue box to remain within a margin inside of the screen space, effectively capping the maximum positions of each bound of the positions, preferentially without jumping. Let’s see how we’ll do that.

In our beloved `UIManager` script, we first add a field that will allow the designer to adjust the margin size.

For our facility, let’s also declare fields that will contain the bounding box values. We will also need to know where are the bounds of our box.

```csharp
    [Header("Dialogue Box Behaviour Settings")]
    [Tooltip("Safe margin area between the dialogue box and the screen limits")]
    [SerializeField] private float screenMargin = 15f;
	// screen bounds
    private float boundTop;
    private float boundRight;
    private float boundBottom;
    private float boundLeft;
	// box bounds
	private float boxBoundTop;
    private float boxBoundRight;
    private float boxBoundBottom;
    private float boxBoundLeft;
```

At start, we will need to know what are our screen bounds. Those will depend on the screen resolution, hopefully unscaled. We do pick them from the parent canvas width and height—to do so, we do need a reference to our current UI Canvas.

 ```csharp
     [SerializeField] private Canvas uiCanvas;
 ```

Then at start:

```csharp
    void Start()
    {
        // ...

        boundTop = uiCanvas.GetComponent<RectTransform>().rect.height - screenMargin;
        boundRight = uiCanvas.GetComponent<RectTransform>().rect.width - screenMargin;
        boundBottom = screenMargin;
        boundLeft = screenMargin;
        
        // ...
        
    }
```

However, since the bounding box of our dialogue box keeps moving, we need to keep track of it during update.

To calculate the coordinate of each bound, we need the coordinates of the box origin point that we add to the width or height of the box itself. Fortunately our script already contains a reference to the box `RectTransform`, so let’s just pick it up.

```csharp
    void Update()
    {
        if (currentDialogueAnchor != null) PlaceDialogueBoxInScreen();
        
        boxBoundTop = dialogueBoxGroup.anchoredPosition.y + dialogueBoxGroup.rect.height;
        boxBoundRight = dialogueBoxGroup.anchoredPosition.x + dialogueBoxGroup.rect.width;
        boxBoundBottom = dialogueBoxGroup.anchoredPosition.y;
        boxBoundLeft = dialogueBoxGroup.anchoredPosition.x;
    }
```

Note that it comes after the positioning in the anchor. Also, we can already abstract this code to call it as a method instead of hard-writing it in the `Update()`. We can in fact do the same for the code we wrote in the `Start()`.

How much prettier and clearer this is!

Note: Some might find it annoying to abstract methods when they’re only used once in the script rather than just going for it, but I really do prefer working that way as it doesn’t affect performance in this context, is flexible, is clear, and is easily maintainable. Someone who is reviewing the code (or yourself) can easily understand what is going on without even having to use comments, and you can check if it’s really the only time you think you’re using it with the reference tracker in the IDE.

 ```cs
     void Start()
     {
         // ...
 
         GetScreenBoundsWithMargins();
         
         // ...
     }
 
 	void Update()
     {
         if (currentDialogueAnchor != null)
         {
             PlaceDialogueBoxInScreen();
             TrackDialogueBoxBounds();
             RepositionDialogueBoxOnScreen();
         }
     }
 ```

Also, it is very clear now that what we want to achieve is to reposition the dialogue box on the screen. So let’s work on this new method.

Now that we have all our variables, let’s compare them against each other. The expected behaviour is that the dialogue box gets blocked by the exterior margin. This has to be tested and acted on for each limit.

```csharp
    private void RepositionDialogueBoxOnScreen()
    {
        // declaring new field with default values
        float newX = dialogueBoxGroup.anchoredPosition.x;
        float newY = dialogueBoxGroup.anchoredPosition.y;

        // test top bound
        if (boxBoundTop > boundTop)
        {
            newY = dialogueBoxGroup.anchoredPosition.y - (boxBoundTop - boundTop);
        }

        // test bottom bound
        if (boxBoundBottom < boundBottom)
        {
            newY = boundBottom;
        }

        // test right bound
        if (boxBoundRight > boundRight)
        {
            newX = dialogueBoxGroup.anchoredPosition.x - (boxBoundRight - boundRight);
        }

        // test left bound
        if (boxBoundLeft < boundLeft)
        {
            newX = boundLeft;
        }

        dialogueBoxGroup.anchoredPosition = new Vector2(newX, newY);
    }
```

I do expect this code to only work if the anchored position pivot is set to the bottom left, because the actual box bounds positions will depend on whether one should add or subtract their width to the position.

This is actually not the case here—the current code assigns a bottom left pivot if we’re using the player speech slot because we assume it to be on the right hand side, and a bottom right pivot if we’re using the character speech slot because we assume it to be on the left hand side. This has been set in our first phase of our *super basic draft*.

The reason that we used two anchors was to give that back and forth sensation, by displaying stuff on the right hand or left hand side. For now, it is simply assumed that the player’s dialogue is on the right, and the other characters dialogue is on the left. This assumption is forced down manually, and obviously ignores the effective position of the player—or its anchor—on the screen.

So if by example the player is on the right and the character they’re speaking to is on the right too, the player’s dialogue went off window and the other character’s dialogue went over it. Now that it is guarded by the bounds, everyone will get covered according to their anchors. Not good. We want it to be constantly constrained, no matter its setting.

So we can already correct our `TrackDialogueBoxBounds()` to account for all types of pivot.

```csharp

	 private void TrackDialogueBoxBounds()
    {

        // pivot horizontal
        if (dialogueBoxGroup.pivot.x < 0.5f)
        {
            boxBoundLeft = dialogueBoxGroup.anchoredPosition.x;
            boxBoundRight = dialogueBoxGroup.anchoredPosition.x + dialogueBoxGroup.rect.width;
        }
        else
        {
            boxBoundLeft = dialogueBoxGroup.anchoredPosition.x - dialogueBoxGroup.rect.width;
            boxBoundRight = dialogueBoxGroup.anchoredPosition.x;
        }

        // pivot vertical
        if (dialogueBoxGroup.pivot.y < 0.5f)
        {
            boxBoundTop = dialogueBoxGroup.anchoredPosition.y + dialogueBoxGroup.rect.height;
            boxBoundBottom = dialogueBoxGroup.anchoredPosition.y;
        }
        else
        {
            boxBoundTop = dialogueBoxGroup.anchoredPosition.y;
            boxBoundBottom = dialogueBoxGroup.anchoredPosition.y - dialogueBoxGroup.rect.height;
        }
    }
```

Then also change our code that replaces the boxes:

```csharp
    private void RepositionDialogueBoxOnScreen()
    {
        // declaring new field with default values
        float newX = dialogueBoxGroup.anchoredPosition.x;
        float newY = dialogueBoxGroup.anchoredPosition.y;

        // rules for left pivot
        if (dialogueBoxGroup.pivot.x < 0.5)
        {
            if (boxBoundRight > boundRight)
                newX = dialogueBoxGroup.anchoredPosition.x - (boxBoundRight - boundRight);

            if (boxBoundLeft < boundLeft)
                newX = boundLeft;
        }
        // rules for right pivot
        else
        {
            if (boxBoundRight > boundRight)
                newX = boundRight;

            if (boxBoundLeft < boundLeft)
                newX = boundLeft + dialogueBoxGroup.rect.width;
        }

        // rules for bottom pivot
        if (dialogueBoxGroup.pivot.y < 0.5)
        {
            if (boxBoundTop > boundTop)
             newY = dialogueBoxGroup.anchoredPosition.y - (boxBoundTop - boundTop);

            if (boxBoundBottom < boundBottom)
                newY = boundBottom;
        }
        // rules for top pivot
        else
        {
            if (boxBoundTop > boundTop)
                newY = boundTop;
            
            if (boxBoundBottom < boundBottom)
                newY = boundBottom + dialogueBoxGroup.rect.height;
        }

        dialogueBoxGroup.anchoredPosition = new Vector2(newX, newY);
    }
```

Now, no matter where the box is placed on the screen or what is its pivot, it will always be constrained within the screen.

The next step is making the dialogue box avoid covering the characters on screen. To do it so, we are first going to test it with the player only.

The behaviour that we want is to actually be as close as possible to the anchor, as far as possible from the screen limit, but without covering anything. To do so, we first need to know where we are on screen and where is the most space available. Then, we will determine the pivot position depending on which width and height is available on each direction.

So first, let’s first test for the space available.

```csharp
    public void PlaceDialogueBoxInScreen()
    {
        // world position to screen
        Vector2 dialogueScreenPosition = Camera.main.WorldToScreenPoint(currentDialogueAnchor.position);

        // space/distance around the coordinates
        float spaceLeft = dialogueScreenPosition.x - boundLeft;
        float spaceRight = boundRight - dialogueScreenPosition.x;
        float spaceTop = dialogueScreenPosition.y - boundBottom;
        float spaceBottom = boundTop - dialogueScreenPosition.y;
        
        // ...
    }
```

Then, assigning the pivots and positions.

```csharp
	public void PlaceDialogueBoxInScreen()
    {
        
        // ...

		float pivotX;
        float pivotY;

        // setting the pivots
        if (spaceLeft > spaceRight)
            pivotX = 0;
        else pivotX = 1;

        if (spaceTop > spaceBottom)
            pivotY = 0;
        else pivotY = 1;

        // assigning the pivot
        dialogueBoxGroup.pivot = new Vector2(pivotX, pivotY);
        
        // assigning the position
        dialogueBoxGroup.anchoredPosition = dialogueScreenPosition;
        
    }
```

However, the script still looks weird. The position sent back by `Camera.WorldToScreenPoint()` goes beyond the bounds of our canvas, this is because the resolution of the camera is higher than the Canvas Scaler reference resolution. It gives a position that is way too high, because it’s not on the same space.

We so have to scale down the values. It is a simple rule of three. Note that the distances from the sides of the screen were also affected by this; they are replaced by the scaled values.

```csharp
    public void PlaceDialogueBoxInScreen()
    {
        // world position to screen
        Vector2 dialogueScreenPosition = Camera.main.WorldToScreenPoint(currentDialogueAnchor.position);

        float pivotX;
        float pivotY;
        
		// scale screen position to canvas position
        float posX = dialogueScreenPosition.x / (Camera.main.pixelWidth / (boundRight + screenMargin));
        float posY = dialogueScreenPosition.y / (Camera.main.pixelHeight / (boundTop + screenMargin));

        // space/distance around the coordinates
        float distToTop = boundTop - posY - boundBottom;
        float distToRight = boundRight - posX - boundLeft;
        float distToLeft = posX - boundLeft;
        float distToBottom = posY - boundBottom;


        // setting the pivots
        if (distToLeft > distToRight)
            pivotX = 0;
        else pivotX = 1;

        if (distToTop > distToBottom)
            pivotY = 0;
        else pivotY = 1;

        // assigning the pivot
        dialogueBoxGroup.pivot = new Vector2(pivotX, pivotY);
        
        // assigning the position
        dialogueBoxGroup.anchoredPosition = new Vector2(posX, posY);

    }
```



Now, we want the dialogue box to always place around the player bounding box, never on it. We first have to fetch the 8 corners of a bounding box that encapsulates the player by calculating the distance from the centre, because that’s how the Unity `Bounds` do.

Since the player mesh is animated, it will be moving all the time and we don’t want the dialogue boxes to move because of the animations. For that reason, we’re going to use the bounds of the associated collider.

We then convert all the positions into screen points and calculate the minimums and maximums to define the canvas bounds. Note that I also used the opportunity to abstract the scaling since we’re doing it all the time.

```csharp
public void PlaceDialogueBoxInScreen()
    {
        // bounds of the player collider
        Vector3 pC = playerCollider.bounds.center;
        Vector3 pE = playerCollider.bounds.extents;

        // bounds corners positions in world space
        Vector3[] pCornersWS = new []
        {
            new Vector3( pC.x + pE.x, pC.y + pE.y, pC.z + pE.z ),
            new Vector3( pC.x + pE.x, pC.y + pE.y, pC.z - pE.z ),
            new Vector3( pC.x + pE.x, pC.y - pE.y, pC.z + pE.z ),
            new Vector3( pC.x + pE.x, pC.y - pE.y, pC.z - pE.z ),
            new Vector3( pC.x - pE.x, pC.y + pE.y, pC.z + pE.z ),
            new Vector3( pC.x - pE.x, pC.y + pE.y, pC.z - pE.z ),
            new Vector3( pC.x - pE.x, pC.y - pE.y, pC.z + pE.z ),
            new Vector3( pC.x - pE.x, pC.y - pE.y, pC.z - pE.z )
        };

        // bounds corners positions in canvas space
        Vector2[] pCornersCS = new Vector2[8];


        // convert world corner points to screen corners and scale
        for (int i = 0; i < pCornersWS.Length; i++)
        {
            pCornersCS[i] = WorldToCanvasPoint(pCornersWS[i], Camera.main);
        }

        // initialize canvas space bounds
        float playerBoundRight = pCornersCS[0].x;
        float playerBoundLeft = pCornersCS[0].x;
        float playerBoundBottom = pCornersCS[0].y;
        float playerBoundTop = pCornersCS[0].y;

        // find extremes
        for (int i = 1; i < pCornersCS.Length; i++)
        {
            if (pCornersCS[i].x > playerBoundRight)
                playerBoundRight = pCornersCS[i].x;
            
            if (pCornersCS[i].x < playerBoundLeft)
                playerBoundLeft = pCornersCS[i].x;
            
            if (pCornersCS[i].y > playerBoundTop)
                playerBoundTop = pCornersCS[i].y;

            if (pCornersCS[i].y < playerBoundBottom)
                playerBoundBottom = pCornersCS[i].y;
        }


        // world position to screen
        Vector2 screenPos = WorldToCanvasPoint(currentDialogueAnchor.position, Camera.main);
        
        // space/distance around the coordinates
        float distToTop = boundTop - screenPos.y - boundBottom;
        float distToRight = boundRight - screenPos.x - boundLeft;
        float distToLeft = screenPos.x - boundLeft;
        float distToBottom = screenPos.y - boundBottom;

        float pivotX;
        float pivotY;

        // setting the pivots
        if (distToLeft > distToRight)
            pivotX = 0;
        else pivotX = 1;

        if (distToTop > distToBottom)
            pivotY = 0;
        else pivotY = 1;

        // assigning the pivot
        dialogueBoxGroup.pivot = new Vector2(pivotX, pivotY);
        
        // assigning the position
        dialogueBoxGroup.anchoredPosition = new Vector2(screenPos.x, screenPos.y);

    }

// ...

    private Vector2 WorldToCanvasPoint(Vector3 position, Camera camera)
    {
        Vector2 coordinate = camera.WorldToScreenPoint(position);

        float x = coordinate.x / (Camera.main.pixelWidth / (boundRight + screenMargin));
        float y = coordinate.y / (Camera.main.pixelHeight / (boundTop + screenMargin));

        return new Vector2(x, y);
    }
```

Now that we have the bounding box of our player, we can offset the dialogue box anchor outside of it.

For this, we need to know how far is our anchor point from the bound that is on the same side as the side that has the largest area between the anchor and the screen bounds. That is a lot of intricate comparisons, but we’ve already done a few of them!
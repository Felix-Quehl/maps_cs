# Maps Tool

This tool can download tailes from a Open Street Map Tiles Server and render them into a single PNG.

## How to use it

```powershell
.\maps.exe `
    --name=ukrain.png `
    --server=https://tile.openstreetmap.org `
    --cache=openstreetmap `
    --zoom=10 `
    --top=53 `
    --bottom=44 `
    --left=21 `
    --right=41
```

## Arguments

```bash
    --name      # name of the output png
    --server    # full url prefix path of tile data like https://server.domain/tiles
    --cache     # directory to cache tiles
    --zoom      # OSM zoom level
    --top       # latitude of the upper map boundary 
    --bottom    # latitude of the lower map boundary 
    --left      # longitude of the left map boundary 
    --right     # longitude of the right map boundary 
```

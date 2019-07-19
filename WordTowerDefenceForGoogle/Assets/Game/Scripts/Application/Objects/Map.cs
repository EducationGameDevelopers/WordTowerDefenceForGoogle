using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 鼠标点击参数类
/// </summary>
public class TileClickEventArgs : EventArgs
{
    public int MouseButton;   //0鼠标左键，1鼠标右键
    public Tile Tile;
    public bool IsOnUI;      //是否在UI上
    public TileClickEventArgs(int mouseButton, Tile tile,bool isOnUI)
    {
        this.MouseButton = mouseButton;
        this.Tile = tile;
        this.IsOnUI = isOnUI;
    }
}

public class Map : MonoBehaviour
{
    #region 常量
    public const int RowCount = 9;   //行数
    public const int ColumnCount = 16;  //列数
    #endregion

    #region 事件
    //格子点击委托事件
    public event EventHandler<TileClickEventArgs> OnTileClick;
    #endregion

    #region 字段
    private float mapWidth;   //地图宽
    private float mapHeight;  //地图高

    private float tileWidth;  //格子宽
    private float tileHeight; //格子高

    private Level m_level;    //关卡数据

    private List<Tile> m_grid = new List<Tile>(); //所有格子集合
    private List<Tile> m_road = new List<Tile>(); //敌人路径集合

    public bool isDrawGizmos = true;   //是否绘制网格
    public int themeIndex = 0;
    #endregion

    #region 属性

    public Level Level
    { get { return m_level; } }

    //背景图片加载设置
    public string BackgroundImage
    {
        set
        {
            SpriteRenderer render = transform.Find("Background").GetComponent<SpriteRenderer>();
            StartCoroutine(Tools.LoadImage(value, render));
        }
    }

    //路径图片加载设置
    public string RoadImage
    {
        set
        {
            SpriteRenderer render = transform.Find("Road").GetComponent<SpriteRenderer>();
            StartCoroutine(Tools.LoadImage(value, render));
        }
    }

    public Rect MapRect
    {
        get
        {
            return new Rect(-mapWidth / 2, -mapHeight / 2, mapWidth, mapHeight);
        }
    }

    public List<Tile> Grid
    {
        get { return m_grid; }
    }

    public List<Tile> Road
    {
        get { return m_road; }
    }


    /// <summary>
    /// 敌人行进路径的坐标集合
    /// </summary>
    public Vector3[] Path()
    {
        List<Vector3> m_path = new List<Vector3>();
        //将路径格子转化为坐标位置存入新的集合中
        for (int i = 0; i < m_road.Count; i++)
        {
            Tile t = m_road[i];
            Vector3 postion = GetGridPostion(t);
            m_path.Add(postion);
        }
        return m_path.ToArray();
    }

    /// <summary>
    /// 怪物的生成位置
    /// </summary>
    public Vector3 MonsterSpawnPostion()
    {
        return GetGridPostion(m_road[0]);
    }

    /// <summary>
    /// 萝卜出生位置(路径终点位置)
    /// </summary>
    /// <returns></returns>
    public Vector3 LuoBoSpawnPostion()
    {
        return GetGridPostion(m_road[m_road.Count - 1]);
    }
    #endregion

    #region 方法
    /// <summary>
    /// 清除塔位信息
    /// </summary>
    public void ClearHolder()
    {
        foreach (Tile t in m_grid)
        {
            if (t.CanHold)
            {
                t.CanHold = false;
            }
        }       
    }

    /// <summary>
    /// 清除敌人路径信息
    /// </summary>
    public void ClearRoad()
    {
        m_road.Clear();
    }

    /// <summary>
    /// 清除所有信息
    /// </summary>
    public void ClearAll()
    {
        m_level = null;
        ClearHolder();
        ClearRoad();
    }

    /// <summary>
    /// 加载当前关卡信息
    /// </summary>
    public void LoadLevel(Level level)
    {
        //清空之前的信息
        ClearAll();
        //保存传递过来的关卡信息
        m_level = level;

        //加载图片资源
//#if UNITY_EDITOR
        //this.BackgroundImage = "file://" + Consts.MapDir + "/" + level.Background;
        //this.RoadImage = "file://" + Consts.MapDir + "/" + level.Road;

//#elif UNITY_ANDROID
        string[] backgroundName = level.Background.Split('.');
        string[] roadName = level.Road.Split('.');
        this.BackgroundImage = "Maps" + "/" + backgroundName[0];
        this.RoadImage = "Maps" + "/" + roadName[0];
//#endif

        //加载敌人路径
        for (int i = 0; i < level.Path.Count; i++)
        {
            Point p = level.Path[i];
            Tile t = GetTile(p.X, p.Y);
            
            m_road.Add(t);
        }

        //加载塔的放置点
        for (int i = 0; i < level.Holder.Count; i++)
        {
            Point p = level.Holder[i];
            Tile t = GetTile(p.X, p.Y);
            t.CanHold = true;
        }
    }
    #endregion

    #region Unity回调

    void Awake()
    {
        //计算地图和格子大小
        CalculateSize();
        
        //创建所有的格子
        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                m_grid.Add(new Tile(j, i));
            }
        }

        OnTileClick += Map_OnTileClick;
    }

    void Update()
    {
#if UNITY_EDITOR
        //鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            //获得此时鼠标下的格子
            Tile tile = GetTileUnderMouse();
            if (tile != null)
            {
                bool isOnUI = EventSystem.current.IsPointerOverGameObject();
                //触发鼠标左键点击事件
                TileClickEventArgs e = new TileClickEventArgs(0, tile, isOnUI);
                if (OnTileClick != null)
                {
                    OnTileClick(this, e);
                }
            }
        }

        //鼠标左键点击
        if (Input.GetMouseButtonDown(1))
        {
            //获得此时鼠标下的格子
            Tile tile = GetTileUnderMouse();
            if (tile != null)
            {
                //触发鼠标右键点击事件
                TileClickEventArgs e = new TileClickEventArgs(1, tile, false);
                if (OnTileClick != null)
                {
                    OnTileClick(this, e);
                }
            }
        }
#endif

#if UNITY_ANDROID ||UNITY_IOS && !UNITY_EDITOR
    //手指点击
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //获得此时手指下的格子
                Tile tile = GetTileUnderTouch();
                if (tile != null)
                {
                    bool isOnUI = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                    //触发鼠标左键点击事件
                    TileClickEventArgs e = new TileClickEventArgs(0, tile, isOnUI);
                    if (OnTileClick != null)
                    {
                        OnTileClick(this, e);
                    }
                }
            }
        }
#endif
    }

    /// <summary>
    /// 图标绘制显示(只在编辑器中起作用)
    /// </summary>
    void OnDrawGizmos()
    {
        if (!isDrawGizmos)
            return;

        //计算地图和格子大小
        CalculateSize();

        //格子绘制线为绿色
        Gizmos.color = Color.green;

        //绘制行
        for (int row = 0; row <= RowCount; row++)
        {
            Vector2 from = new Vector2(-mapWidth / 2, -mapHeight / 2 + row * tileHeight);
            Vector2 to = new Vector2(-mapWidth / 2 + mapWidth, -mapHeight / 2 + row * tileHeight);
            Gizmos.DrawLine(from, to);
        }

        //绘制列
        for (int col = 0; col <= ColumnCount; col++)
        {
            Vector2 from = new Vector2(-mapWidth / 2 + col * tileWidth, mapHeight / 2);
            Vector2 to = new Vector2(-mapWidth / 2 + col * tileWidth, -mapHeight / 2);
            Gizmos.DrawLine(from, to);
        }

        //绘制可放塔位的格子
        foreach (Tile t in m_grid)
        {
            if (t.CanHold == true)
            {
                Vector3 pos = GetGridPostion(t);
                Gizmos.DrawIcon(pos, "holder.png", true);
            }
        }

        //路径线为红色
        Gizmos.color = Color.red;
        //绘制路径
        for (int i = 0; i < m_road.Count; i++)
        {
            //起点
            if (i == 0)
            {
                //路径起点图标
                Gizmos.DrawIcon(GetGridPostion(m_road[i]), "start.png", true);
            }

            //终点
            if (m_road.Count > 0 && i == m_road.Count - 1)
            {
                //路径终点坐标
                Gizmos.DrawIcon(GetGridPostion(m_road[i]), "end.png", true);
            }

            //红色的路径连线
            if (m_road.Count > 1 && i != 0)
            {
                Vector3 from = GetGridPostion(m_road[i - 1]);
                Vector3 to = GetGridPostion(m_road[i]);
                Gizmos.DrawLine(from, to);
            }
        }
    }
    #endregion

    #region 事件回调
    /// <summary>
    /// 地图方格点击事件处理
    /// </summary>
    void Map_OnTileClick(object sender, TileClickEventArgs e)
    {

        if (gameObject.scene.name != "LevelBuilder")
            return;

        if (Level == null)
            return;

        //点击鼠标左键并且此时点击的格子不是路径点
        if (e.MouseButton == 0 && !m_road.Contains(e.Tile))
        {
            //处理放置塔点
            e.Tile.CanHold = !e.Tile.CanHold;
        }

        //点击鼠标右键且此时点击的格子不是放塔点
        if (e.MouseButton == 1 && e.Tile.CanHold == false)
        {
            //敌人路径处理
            if (m_road.Contains(e.Tile))
            {
                m_road.Remove(e.Tile);
            }
            else
            {
                m_road.Add(e.Tile);
            }
        }
    }
    #endregion

    #region 帮助方法
    /// <summary>
    /// 计算地图大小，格子大小
    /// </summary>
    private void CalculateSize()
    {
        //屏幕归一化坐标
        Vector3 leftDown = new Vector3(0, 0);
        Vector3 rightUp = new Vector3(1, 1);
        //屏幕视口坐标转为世界坐标
        Vector3 p1 = Camera.main.ViewportToWorldPoint(leftDown);
        Vector3 p2 = Camera.main.ViewportToWorldPoint(rightUp);

        //计算地图宽和高
        mapWidth = Math.Abs(p2.x - p1.x);
        mapHeight = Math.Abs(p2.y - p1.y);

        //计算每个格子的宽和高
        tileWidth = mapWidth / ColumnCount;
        tileHeight = mapHeight / RowCount;
    }

    /// <summary>
    /// 获取格子中心点所在的世界坐标
    /// </summary>
    public Vector3 GetGridPostion(Tile t)
    {
        return new Vector3(-mapWidth / 2 + (t.X + 0.5f) * tileWidth,
                           -mapHeight / 2 + (t.Y + 0.5f) * tileHeight,
                           0);
    }

    /// <summary>
    /// 根据格子索引获取格子
    /// </summary>
    public Tile GetTile(int tileX, int tileY)
    {
        //根据各自坐标获取其索引值
        int index = tileX + tileY * ColumnCount;
        if (index < 0 || index >= m_grid.Count)
        {
            throw new IndexOutOfRangeException("格子索引越界");
        }
        return m_grid[index];
    }
    /// <summary>
    /// 根据格子所在位置获取格子
    /// </summary>
    public Tile GetTile(Vector3 postion)
    {
        int tileX = (int)((postion.x + mapWidth / 2) / tileWidth);
        int tileY = (int)((postion.y + mapHeight / 2) / tileHeight);
        return GetTile(tileX, tileY);
    }
    /// <summary>
    /// 获取鼠标下面的格子
    /// </summary>
    /// <returns></returns>
    private Tile GetTileUnderMouse()
    {
        Vector2 worldPos = GetWorldPostion();
        return GetTile(worldPos);
    }

    /// <summary>
    /// 获取鼠标所在的世界坐标
    /// </summary>
    private Vector3 GetWorldPostion()
    {
        //鼠标所在的视口坐标
        Vector3 viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //视口坐标转化为世界坐标
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        return worldPos;
    }

    /// <summary>
    /// 获取触摸点击下面的格子
    /// </summary>
    /// <returns></returns>
    private Tile GetTileUnderTouch()
    {
        Vector2 viewPos = Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position);
        //视口坐标转化为世界坐标
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        return GetTile(worldPos);
    }

    #endregion
}


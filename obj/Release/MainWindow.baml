   M S B A M L   `   `   `  ���� C  ?WorldGen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:WorldGenWorldGen      WorldGen.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     2gif"http://wpfanimatedgif.codeplex.com     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:WorldGen     H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null6mah,http://metro.mahapps.com/winfx/xaml/controls  -     � Title$  WorldGen��$	��768��$
��1366�� � Cursor$ Arrow�� � WindowStyle$
 None=� � WindowState$ 	Maximized=�.��� -   $	��app�� J� RenderTransformOrigin$ 0.498,0.481&�.+�� -   $
��menu��$
��Left=�$��0q�$��50�����	#FF383838 K� ZIndex$ -1��.+��� -    � ToolTip$ Monde��$
��Bold"�$��40��$��40��$��Center=�$	��Top=�$ 1��$��0,10,0,0q���Z� ��Z� ����  �� ImageSource$ Assets/menu/map.png�� j� Stretch$ Uniform=��� -   $
 Pays��$
��Bold"�$��40��$��40��$��Center=�$	��Top=�$��0,60,0,0q�$ 1����Z� ��Z� ���� $ Assets/menu/kingdom.png��$ Uniform=��� -   $ Villes��$
��Bold"�$��40��$��40��$��Center=�$	��Top=�$��	0,120,0,0q�$ 1����Z� ��Z� ���� $ Assets/menu/city.png��$ Uniform=��� -   $ 	Panthéon��$
��Bold"�$��40��$��40��$��Center=�$	��Top=�$��	0,180,0,0q�$ 1����Z� ��Z� ���� $ Assets/menu/temple.png��$ Uniform=��� -   $ Quitter��$
��Bold"�$ 0.5,0.5&�$��40��$��40��$��Center=�$	��Top=�$��	0,360,0,0q�$ 1����Z� ��Z� ���� $ Assets/menu/off.png��$ Uniform=��� -	   $ Sauvegarder��$��40��$��40��$��	0,240,0,0q�$��Center=�$	��Top=���Z� ��Z� ���� $ Assets/menu/save.png��$ Uniform=��� $ Ouvrir��$��40��$��40��$��Center=�$	��Top=�$��	0,300,0,0q���Z� ��Z� ���� $ Assets/menu/open.png��$ Uniform=�� -
   $	��map��$y�Hidden=�$��50,0,0,0q�$��Center=�$��Center=�.+��� -   $��imageMap��$��Assets/Image/map.jpg��$��0q�$
��Fill=�$ Cross���� -   	 J� Opacity$
	 0.75Q�$��Nouveau monde��$��Bottom=�$
��Left=�$	��125��$��25��$��20,0,0,5q��� -   $��Right=�$��0,0,5,5q�$��Bottom=�$��32��$��32��$
 Hand����Z� ��Z� ���� $ Uniform=�$ Assets/menu/bars.png��� -   $��
subMenuMap��$y�Hidden=�$��Right=�$	��138��$��
0,0,5,42.2q�$��Bottom=�$	��119�����	#BF000000.+��� -   $
	 0.75Q�$��Delaunay: OFF��$��Bottom=�$
��Left=�$	��100��$��25��$��
10,0,0,103q��� -   $
	 0.75Q�$��Voronoi: ON��$��Bottom=�$
��Left=�$	��100��$��25��$��	10,0,0,73q��� -   $
	 0.75Q�$��Royaume: ON��$��Bottom=�$
��Left=�$	��100��$��25��$��	10,0,0,43q��� -   $
	 0.75Q�$��Croyance: OFF��$��Bottom=�$
��Left=�$	��100��$��25��$��	10,0,0,13q�� -   $
��city��$y�Hidden=�$��50,0,0,0q�$��Center=�$��Center=�.+��� -   $��backgroundCity��$,��&Assets/Background/BackgroundCity-0.jpg��$
��Fill=��� -   $��
isCapitale��$y�Hidden=�$
��Left=�$��38��$��	10,10,0,0q�$	��Top=�$��38��$��Assets/Image/crown.png���� -   $��isNotCapitale��$y�Hidden=�$
��Left=�$��38��$��	10,10,0,0q�$	��Top=�$��38��$��Assets/Image/city.png��$
��Fill=��� -   $��listCity��$
��Left=�$��	53,10,0,0q�$	��Top=�$	��190��$��38��$��Viner Hand ITC)�$��18(�$
��Bold"�
��������� -   $��cityKingdom��$��Label��$
��Left=�$��	10,53,0,0q�$	��Top=�$ 0.521,-0.609&�$	��233��$��Viner Hand ITC)�$
��Bold"�$��18(�
���   ��� -   $��cityInfo��$
��Left=�$	��201��$��
10,182,0,0q�$	��Top=�$	��233��
 �� SelectionBrush
 �	#BF0078D7$��Viner Hand ITC)�$
��Bold"�$��18(�
���   ���Z� ��Z� . �/� .T�J� .*�.*��� $�RichTextBox���� -   $��cityGod��$��Label��$
��Left=�$��	10,96,0,0q�$	��Top=�$	��233��$��Viner Hand ITC)�$
��Bold"�$��18(�
���   ��� -   $��cityType��$��Label��$
��Left=�$��
10,139,0,0q�$	��Top=�$	��233��$��18(�$��Viner Hand ITC)�$
��Bold"�
���   ��� -   $��cityRessource��$��Ressource principale: ��$
��Left=�$��
250,10,0,0q�$	��Top=�$��38��$��18(�$
��Bold"�$��Viner Hand ITC)�$	��350��
���   �� -   $��kingdom��$y�Visible=�$��50,0,0,0q�.+��� -   $��backgroundKingdom��$
��Fill=�$��0q��� -    ��Name$ listKingdom��$��10,5,0,0q�$
��Left=�$	��Top=�$	��300��$��50��$
��Bold"�$��24(�$��Viner Hand ITC)��� -    $��capitalName��$��Label��$
��Left=�$��35��$��	10,60,0,0q�$	��Top=�$	��300��$��18(�$
��Bold"�
���   �$��Viner Hand ITC)���Z� �� -!   $��demoInfo��$
��Left=�$	��250��$��
10,140,0,0q�$	��Top=�$	��300�����	#3F000000 �� 
IsReadOnly . $
��Bold"�$��18(�$��Viner Hand ITC)�
���   ���Z� . �/� .T�J� .*�.*��� $�RichTextBox���� -"   $��kingdomType��$��Label��$
��Left=�$��
10,100,0,0q�$	��Top=�$	��450��$��35��$��18(�$
��Bold"�$��Viner Hand ITC)�
���   ��� -#    ��Name$ ecoInfo��$
��Left=�$	��250��$��315,140,0,0q�$	��Top=�$	��300�����	#3F000000
���   �$��Viner Hand ITC)�$
��Bold"�$��18(�. �/� .T�J� .*�.*��� $�RichTextBox��� -$   $��pantheon��$y�Hidden=�$��50,0,0,0q�.+��� -%   $��backgroundPantheon��$,��&Assets/Background/BackgroundTemple.jpg��$
��Fill=�$��0q��� -&   $��pantheonContent��$
��Left=�$��0q�$	��400��$��Viner Hand ITC)�$��18(�$
��Bold"���Z� ��Z� �� -'   $��nbRegion��$��Label��$
��Left=�$��
405,10,0,0q�$	��Top=�$	��250��$��Viner Hand ITC)�$��18(�$
��Bold"��� -(   $��
nbFollower��$��Label��$
��Left=�$��
405,53,0,0q�$	��Top=�$	��250��$��Viner Hand ITC)�$��18(�$
��Bold"�
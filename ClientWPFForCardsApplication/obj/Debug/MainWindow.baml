   M S B A M L   `   `   `  ����W  SClientWPFForCardsApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:ClientWPFForCardsApplicationClientWPFForCardsApplication  5   	   -    'ClientWPFForCardsApplication.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:ClientWPFForCardsApplication     5   	   -   5          � Title$  
MainWindow��5   	   $��480.4��6   $��827.2��6+   .��5
      �  � RowDefinitions 5   
   �� 5      $
��277* �6   �� 5      $
��143* �6   .+�5   
   �� $��Visible=�6   .��5      �� -   5         ��Name$ cardsStackPanel��5       �� Orientation$ 
Horizontal=�63   $
��Left=�6L   $	��253��6g   $��0,0,9.6,13.8q�6t   $��Bottom=�6�    �� CanHorizontallyScroll . $��Visible=�6�   $��Disabled=�6�   ��. 5      5      �� 5   
   -   5         ��Name$ Delete��5      $��Delete Selected��6!   $��0,0,97.8,81.4q�6;   $��1��6m   $��Right=�6z   $	��116��6�   $��49��6�   $��Bottom=�6�   �� 5   
   -   5         ��Name$ TextBoxForTitle��5      $��21��6+   $��42,0,0,10.4q�67    �� TextWrapping$
 Wrap=�6L   $��Default��6`   $��Bottom=�6o   $��1��6�   $
��Left=�6�   $	��100��6�   �� 5   
   -   5        $ Add_Card��5      $��Add_Card��6#   $��186,0,0,42.4q�66   $��1��6h   $
��Left=�6u   $��87��6�   $��32��6�   $��Bottom=�6�   �� 5   
   -   5        $ ImageSelection��5      $��Select Image��6)   $��186,0,0,100.4q�6@   $��1��6t   $��32��6�   $��Bottom=�6�   $
��Left=�6�   $��87��6�   �� 5   
   -   5         ��Name$ SelectedImage��5      $
��Left=�6'   $	��100��6B   $��42,11.2,0,0q�6O   $	��Top=�6d   $	��100��6|   $��1��6�   �� 5   
   -   5        $
 Sort��5      $
��Sort��6   $��0,0,235.8,81.4q�6.   $��1��6F   $��49��6f   $��Bottom=�6r   $��Right=�6�   $	��116��6�   5      5      